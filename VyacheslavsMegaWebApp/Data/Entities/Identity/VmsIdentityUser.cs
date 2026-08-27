using Microsoft.AspNetCore.Identity;
using System.Configuration;

namespace VyacheslavsMegaWebApp.Data.Entities.Identity
{
    public class VmsIdentityUser : IdentityUser
    {
        public VmsIdentityUser() : base() { }

        [RegexStringValidator(@"^[A-Za-z0-9_]{3,16}$")]
        public override string? UserName { get => base.UserName; set => base.UserName = value; }

        [RegexStringValidator(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public override string? Email { get => base.Email; set => base.Email = value; }

        public byte[]? EmailConfirmCodeHash { get; set; }

        public DateTime? EmailConfirmedCodeCreationTime { get; set; }
    }
}
