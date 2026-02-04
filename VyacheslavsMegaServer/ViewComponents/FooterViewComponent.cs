
using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaServer.Data.Repositories;
using VyacheslavsMegaServer.Models.ViewComponents;
using VyacheslavsMegaServer.Data.JSON.MCServerStat;
using VyacheslavsMegaServer.Data.JSON.Repositories;

namespace VyacheslavsMegaServer.ViewComponents
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
            MCServerStat MCServerStat = await _MCServerStatRepository.GetMCServerStats();

            return new FooterViewModel()
            {
                ServerMediaLinks = await _mediaLinksRepository.GetAllLinks(),
                MaxPlayers = MCServerStat?.Players?.Max ?? 0,
                OnlinePlayers = MCServerStat?.Players?.Online ?? 0,
                IsServerOnline = MCServerStat?.IsOnline
            };
        }
    }
}
