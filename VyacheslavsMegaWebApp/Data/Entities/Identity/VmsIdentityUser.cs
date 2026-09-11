using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace VyacheslavsMegaWebApp.Data.Entities.Identity
{
    public class VmsIdentityUser : IdentityUser
    {
        public VmsIdentityUser() : base() { }

        public DateTime? EmailConfirmedCodeCreationTime { get; set; }
    }
}
