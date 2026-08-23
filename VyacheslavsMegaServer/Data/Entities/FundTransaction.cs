using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace VyacheslavsMegaServer.Data.Entities
{   
    public class FundTransaction : Base.EntityBase
    {
        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        [Display(Name = "Доп. информация")]
        public string? Note { get; set; }

        #region Navigations

        [Display(Name = "Причина транзакции")]
        public int TransactionPurposeId { get; set; } = 1;

        [ValidateNever]
        public virtual TransactionPurpose Purpose { get; set; }

        #endregion
    }
}
