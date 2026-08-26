using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaWebApp.Data.Entities.Base;

namespace VyacheslavsMegaWebApp.Data.Entities
{
    public class ContactLink : Link
    {
        #region Navigations
        [Display(Name = "Принадлежит контакту:")]
        public int ContactId { get; set; }

        [ValidateNever]
        public virtual Contact? Contact { get; set; }
        #endregion
    }
}
