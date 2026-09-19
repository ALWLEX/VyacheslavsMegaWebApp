using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VyacheslavsMegaWebApp.Data.Entities.Identity;

namespace VyacheslavsMegaWebApp.Data.Entities
{
    public class LauncherSession
    {
        [Key]
        [Column(Order = 0)]
        public string SessionKey { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiresAt { get; set; }

        [ValidateNever]
        public virtual VmsIdentityUser User { get; set; }
    }
}
