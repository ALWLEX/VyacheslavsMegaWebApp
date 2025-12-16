using VyacheslavsMegaServer.Data.Entities;

namespace VyacheslavsMegaServer.Models
{
    public class DonationPageViewModel
    {
        public List<DonationCard> DonationCards { get; set; }

        public bool HasCards => DonationCards != null && DonationCards.Any();

        private Goal? _goal;
        public Goal? Goal
        {
            get
            {
                return _goal is null ? new Goal() { Name = "пока нет, но всё ещё есть мелкие расходы)"} : _goal;
            }
            set { _goal = value; }
        }

        public decimal FundAmount { get; set; }

        public List<FundTransaction> LastTransactions { get; set; }
    }
}
