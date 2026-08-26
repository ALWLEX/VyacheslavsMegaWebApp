using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Areas.Admin.Models;
using VyacheslavsMegaWebApp.Data.Entities;
using VyacheslavsMegaWebApp.Data.Repositories;

namespace VyacheslavsMegaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DonationCardsEditorController : Controller
    {
        private readonly DonationCardsRepository _cardsRepository;

        public DonationCardsEditorController(DonationCardsRepository cardsRepository) 
        {
            _cardsRepository = cardsRepository;
        }

        public async Task<IActionResult> Index()
        {
            DonationCardsEditorViewModel model = new DonationCardsEditorViewModel(await _cardsRepository.GetAllCards());
            return View(model);
        }

        public async Task<IActionResult> CardDetails(int? cardId)
        {
            if (cardId == null)
                return View(new DonationCard());
            return View(await _cardsRepository.GetCardById(cardId.Value));
        }

        [HttpPost]
        public async Task<IActionResult> CardDetails(DonationCard model)
        {
            if (!ModelState.IsValid) return View(model);
            await _cardsRepository.SaveCard(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveCard(int cardId)
        {
            await _cardsRepository.RemoveCardById(cardId);
            return RedirectToAction(nameof(Index));
        }
    }
}
