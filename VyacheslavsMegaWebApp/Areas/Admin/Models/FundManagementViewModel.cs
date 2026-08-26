using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaWebApp.Data.Entities;

namespace VyacheslavsMegaWebApp.Areas.Admin.Models
{
    public class FundManagementViewModel
    {
        public enum FilterBy { DATE, LAST };

        public List<FundTransaction> FundTransactions { get; set; } = new List<FundTransaction>();

        [Display(Name = "Дата начала выборки")]
        public DateTime DateStart { get; set; } = DateTime.Today.AddMonths(-3);

        [Display(Name = "Дата конца выборки")]
        public DateTime DateEnd { get; set; } = DateTime.Today;

        [Display(Name = "Количество транзакций")]
        [Range (1, 500)]
        public int CountTransactions { get; set; } = 25;

        public FilterBy Filter { get; set; } = FilterBy.LAST;
    }
}
