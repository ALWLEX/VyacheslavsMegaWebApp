using Microsoft.AspNetCore.Identity;
using VyacheslavsMegaWebApp.Data.Repositories;

namespace VyacheslavsMegaWebApp.Areas.Admin.Models
{
    public class UserViewModel
    {
        public IdentityUser IdentityUserInfo { get; set; }

        public string Role { get; set; }
    }
}
