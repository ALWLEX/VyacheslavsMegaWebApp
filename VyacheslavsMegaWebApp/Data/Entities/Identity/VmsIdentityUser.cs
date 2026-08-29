using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace VyacheslavsMegaWebApp.Data.Entities.Identity
{
    public class VmsIdentityUser : IdentityUser
    {
        public VmsIdentityUser() : base() { }

        public byte[]? EmailConfirmCodeHash { get; set; }

        public DateTime? EmailConfirmedCodeCreationTime { get; set; }
    }
}
