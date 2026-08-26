using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Data.Entities;

namespace VyacheslavsMegaWebApp.Areas.Admin.Models
{
    [Area("Admin")]
    public class DonationCardsEditorViewModel
    {
        List<DonationCard> _donationCards;

        public DonationCardsEditorViewModel(List<DonationCard> donationCards)
        {
            _donationCards = donationCards;
        }

        public List<DonationCard> DonationCards => _donationCards;
    }
}
