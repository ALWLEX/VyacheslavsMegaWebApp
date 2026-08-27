using Microsoft.AspNetCore.Identity;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Data.Repositories;

namespace VyacheslavsMegaWebApp.Areas.Admin.Models
{
    public class UserViewModel
    {
        public VmsIdentityUser IdentityUserInfo { get; set; }

        public string Role { get; set; }
    }
}
