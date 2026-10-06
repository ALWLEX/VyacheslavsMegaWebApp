using Microsoft.EntityFrameworkCore;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Data.Entities;
using System.Security.Cryptography;
using System.Text;

namespace VyacheslavsMegaWebApp.Data.Repositories
{
    public class LauncherSessionRepository : Base.RepositoryBase
    {
        public async Task<VmsIdentityUser> GetUserBySessionKey(string sessionKey)
        {
            byte[] sessionKeyHash = HashSessionKey(sessionKey);
            return (await DB.LauncherSessions.FirstAsync(s => s.SessionKeyHash == sessionKeyHash)).User;
        }

        public async Task<string> CreateSession(VmsIdentityUser user)
        {
            await RemoveSessionsForUser(user);

            string sessionKey = GenerateSessionKey();
            DB.LauncherSessions.Add(new LauncherSession 
            { 
                SessionKeyHash = HashSessionKey(sessionKey),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(14),
            });
            await DB.SaveChangesAsync();
            return sessionKey;
        }

        public async Task<bool> CheckSessionKey(string sessionKey)
        {
            LauncherSession? currentSession = await GetSessionByKey(sessionKey);
            return currentSession != null && currentSession.ExpiresAt > DateTime.UtcNow;
        }

        public async Task RemoveSessionsForUser(VmsIdentityUser user)
        {
            List<LauncherSession> existingSessions = DB.LauncherSessions.Where(s => s.UserId == user.Id).ToList();
            foreach (LauncherSession session in existingSessions)
                DB.Entry(session).State = EntityState.Deleted;
            await DB.SaveChangesAsync();
        }

        #region Private methods

        private byte[] HashSessionKey(string sessionKey)
        {
            return SHA256.HashData(Encoding.UTF8.GetBytes(sessionKey));
        }

        private string GenerateSessionKey()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        private async Task<LauncherSession?> GetSessionByKey(string sessionKey)
        {
            byte[] sessionKeyHash = HashSessionKey(sessionKey);
            return await DB.LauncherSessions.OrderBy(s => s.ExpiresAt).LastOrDefaultAsync(s => s.SessionKeyHash == sessionKeyHash);
        }

        #endregion

    }
}
