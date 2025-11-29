using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaServer.Data.Entities;

namespace VyacheslavsMegaServer.Areas.Admin.Models
{
    public class FundManagementViewModel
    {
        public List<FundTransaction> FundTransactions { get; set; }

        [Display(Name = "Дата начала выборки")]
        public DateTime DateStart { get; set; } = DateTime.Now.AddMonths(-1);

        [Display(Name = "Дата конца выборки")]
        public DateTime DateEnd { get; set; } = DateTime.Now;
    }
}
