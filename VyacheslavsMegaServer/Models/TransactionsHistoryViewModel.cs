using VyacheslavsMegaServer.Data.Entities;

namespace VyacheslavsMegaServer.Models
{
    public class TransactionsHistoryViewModel
    {
        public List<FundTransaction> TransactionsList { get; set; }

        public int TotalCount { get; set; }

        public FundTransaction? MaxIncome { get; set; }

        public FundTransaction? MaxExpence { get; set; }
    }
}
