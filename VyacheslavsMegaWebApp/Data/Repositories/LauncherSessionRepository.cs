using Microsoft.EntityFrameworkCore;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Data.Entities;

namespace VyacheslavsMegaWebApp.Data.Repositories
{
    public class LauncherSessionRepository : Base.RepositoryBase
    {
        public async Task<VmsIdentityUser> GetUserBySessionKey(string sessionKey)
        {
            return (await DB.LauncherSessions.LastAsync(s => s.SessionKey == sessionKey)).User;
        }

        public async Task<string> CreateSessionKey(VmsIdentityUser user)
        {

            string sessionKey = Guid.NewGuid().ToString();
            DB.LauncherSessions.Add(new LauncherSession 
            { 
                SessionKey = sessionKey,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(14),
            });
            await DB.SaveChangesAsync();
            return sessionKey;
        }

        public async Task RemoveSessionKeys(VmsIdentityUser user)
        {
            List<LauncherSession> existingSessions = DB.LauncherSessions.Where(s => s.UserId == user.Id).ToList();
            foreach (LauncherSession session in existingSessions)
                DB.Entry(session).State = EntityState.Deleted; 
            await DB.SaveChangesAsync();
        }
    }
}
