using Microsoft.EntityFrameworkCore;
using VyacheslavsMegaWebApp.Data.Entities.Identity;

namespace VyacheslavsMegaWebApp.Data.Repositories
{
    public class LauncherSessionRepository : Base.RepositoryBase
    {
        public async Task<VmsIdentityUser> GetUserBySessionKey(string sessionKey)
        {
            return (await DB.LauncherSessions.LastAsync(s => s.SessionKey == sessionKey)).User;
        }
    }
}
