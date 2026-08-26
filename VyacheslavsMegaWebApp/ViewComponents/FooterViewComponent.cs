
using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Data.Repositories;
using VyacheslavsMegaWebApp.Models.ViewComponents;
using VyacheslavsMegaWebApp.Data.JSON.MCServerStat;
using VyacheslavsMegaWebApp.Data.JSON.Repositories;

namespace VyacheslavsMegaWebApp.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly ServerMediaLinksRepository _mediaLinksRepository;
        private readonly MCServerStatRepository _MCServerStatRepository;

        public FooterViewComponent(ServerMediaLinksRepository mediaLinksRepository, MCServerStatRepository serverStatRepository) 
        {
            _mediaLinksRepository = mediaLinksRepository;
            _MCServerStatRepository = serverStatRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await InitializeModel());
        }

        public async Task<FooterViewModel> InitializeModel()
        {
            return new FooterViewModel()
            {
                ServerMediaLinks = await _mediaLinksRepository.GetAllLinks(),
                MCServerStat = await _MCServerStatRepository.GetMCServerStats()
            };
        }
    }
}
