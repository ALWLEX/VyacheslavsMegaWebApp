using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaWebApp.Data.Entities.Identity;

namespace VyacheslavsMegaWebApp.Models
{
    public class EmailConfirmationViewModel
    {
        [Display(Name = "Код подтверждения")]
        public string ConfirmationCode { get; set; }

        [ValidateNever]
        public VmsIdentityUser User { get; set; }
    }
}
