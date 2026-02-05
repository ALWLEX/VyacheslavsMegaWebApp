using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaServer.Areas.Admin.Models;
using VyacheslavsMegaServer.Data.Entities;
using VyacheslavsMegaServer.Data.Repositories;

namespace VyacheslavsMegaServer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DownloadButtonsController : Controller
    {
        private readonly DownloadButtonsRepository _downloadButtonssRepository;

        public DownloadButtonsController(DownloadButtonsRepository buttonssRepository)
        {
            _downloadButtonssRepository = buttonssRepository;
        }

        public async Task<IActionResult> Index()
        {
            DownloadButtonsViewModel model = new DownloadButtonsViewModel(await _downloadButtonssRepository.GetAllButtons());
            return View(model);
        }

        public async Task<IActionResult> ButtonDetails(int? buttonId)
        {
            if (buttonId == null)
                return View(new DownloadButton());
            return View(await _downloadButtonssRepository.GetButtonById(buttonId.Value));
        }

        [HttpPost]
        public async Task<IActionResult> ButtonDetails(DownloadButton model)
        {
            if (!ModelState.IsValid) return View(model);
            await _downloadButtonssRepository.SaveButton(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveButton(int buttonId)
        {
            await _downloadButtonssRepository.RemoveButtonById(buttonId);
            return RedirectToAction(nameof(Index));
        }
    }
}
