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
        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name = "Тип транзакции")]
        public TransactionType Type { get; set; } = TransactionType.Income;  // "income" | "expense"

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
