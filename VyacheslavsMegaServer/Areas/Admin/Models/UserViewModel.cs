using Microsoft.AspNetCore.Identity;
using VyacheslavsMegaServer.Data.Repositories;

namespace VyacheslavsMegaServer.Areas.Admin.Models
{
    public class UserViewModel
    {
        public IdentityUser IdentityUserInfo { get; set; }

        public string Role { get; set; }
    }
}
