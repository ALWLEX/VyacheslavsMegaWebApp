using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VyacheslavsMegaServer.Data.Repositories;
using VyacheslavsMegaServer.Models;

namespace VyacheslavsMegaServer.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly MainPageRepository _mainPageRepository;
        private readonly PartnersInfoRepository _partnersInfoRepository;
        private readonly ContactsInfoRepository _contactsInfoRepository;
        private readonly DonationCardsRepository _cardsRepository;
        private readonly ServerMediaLinksRepository _mediaLinksRepository;

        public HomeController(MainPageRepository mainPageRepository, PartnersInfoRepository partnersInfoRepository, ContactsInfoRepository contactsInfoRepository, DonationCardsRepository cardsRepository, ServerMediaLinksRepository mediaLinksRepository)
        {
            _mainPageRepository = mainPageRepository;
            _contactsInfoRepository = contactsInfoRepository;
            _partnersInfoRepository = partnersInfoRepository;
            _cardsRepository = cardsRepository;
            _mediaLinksRepository = mediaLinksRepository;
        }

        public async Task<IActionResult> Index()
        {
            MainPageViewModel model = new MainPageViewModel()
            {
                MainPageData = await _mainPageRepository.GetMainPageData(),
                Creator = await _contactsInfoRepository.GetContactById(1),
                PartnersData = await _partnersInfoRepository.GetAllPartners(),
                MediaLinks = await _mediaLinksRepository.GetAllLinks(),
            };
            ViewBag.PageTitle = model.MainPageData.PageTitle;
            ViewBag.Keywords = model.MainPageData.MetatagKeywords;
            ViewBag.Description = model.MainPageData.MetatagDescription;
            return View(model);
        }

        public async Task<IActionResult> DonationPage() 
        {
            DonationPageViewModel model = new DonationPageViewModel()
            {
                DonationCards = await _cardsRepository.GetAllCards()
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel(ErrorViewModel.PageType.Exception) 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult StatusCode(int code)
        {
            return View("Error", new ErrorViewModel(ErrorViewModel.PageType.StatusCode)
            {
                StatusCode = code,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
