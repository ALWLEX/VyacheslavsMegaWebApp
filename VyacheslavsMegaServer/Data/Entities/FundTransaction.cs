using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VyacheslavsMegaServer.Data.Entities
{
    public enum TransactionType
    {
        Income,
        Expense
    }
    
    public class FundTransaction : Base.EntityBase
    {
        [Display(Name = "Дата")]
        public DateTime Date { get; set; } = DateTime.Now;

        [MaxLength(255)]
        [Display(Name = "Причина")]
        public string Reason { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Сумма")]
        public decimal Amount { get; set; }

        [Display(Name = "Доп. информация")]
        public string Note { get; set; }
    }
}
