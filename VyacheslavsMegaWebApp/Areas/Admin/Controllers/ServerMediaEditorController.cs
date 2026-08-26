using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Areas.Admin.Models;
using VyacheslavsMegaWebApp.Data.Entities;
using VyacheslavsMegaWebApp.Data.Repositories;
using VyacheslavsMegaWebApp.Service;

namespace VyacheslavsMegaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServerMediaEditorController : Controller
    {
        private ServerMediaLinksRepository _serverMediaLinksRepository;
        private readonly ImageManager _imageManager;

        public ServerMediaEditorController(ServerMediaLinksRepository serverMediaLinksRepository, ImageManager imageManager) 
        {
            _serverMediaLinksRepository = serverMediaLinksRepository; 
            _imageManager = imageManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(new LinksEditorViewModel<ServerMediaLink>(await _serverMediaLinksRepository.GetAllLinks()));
        }

        public async Task<IActionResult> LinkDetails(int? linkId)
        {
            ServerMediaLink model;
            if (linkId.HasValue)
                model = await _serverMediaLinksRepository.GetLinkById(linkId.Value);
            else
                model = new ServerMediaLink();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LinkDetails(ServerMediaLink model, IFormFile? linkIcon)
        {
            if (!ModelState.IsValid) return View(model);

            if (linkIcon != null)
            {
                string newImageName = await _imageManager.SaveUniqueFileName(ServerMediaLink.LOGOS_DIR, linkIcon);
                if (!string.IsNullOrEmpty(model.LogoFileName))
                {
                    _imageManager.RemoveImage(ServerMediaLink.LOGOS_DIR, model.LogoFileName);
                }
                model.LogoFileName = newImageName;
            }
            await _serverMediaLinksRepository.SaveLink(model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveLink(int linkId)
        {
            ServerMediaLink linkToRemove = await _serverMediaLinksRepository.GetNoTrackedLinkById(linkId);
            await _serverMediaLinksRepository.RemoveLinkById(linkId);
            _imageManager.RemoveImage(ServerMediaLink.LOGOS_DIR, linkToRemove.LogoFileName);
            return RedirectToAction(nameof(Index));
        }
    }
}
