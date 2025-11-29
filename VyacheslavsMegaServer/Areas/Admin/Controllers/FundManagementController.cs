using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaServer.Areas.Admin.Models;
using VyacheslavsMegaServer.Data.Repositories;

namespace VyacheslavsMegaServer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FundManagementController : Controller
    {
        private readonly FundRepository _fundRepository;

        public FundManagementController(FundRepository fundRepository) 
        {
            _fundRepository = fundRepository;   
        }

        public async Task<IActionResult> Index()
        {
            FundManagementViewModel model = new FundManagementViewModel();
            model.FundTransactions = await _fundRepository.GetTransactionsByDate(model.DateStart, model.DateEnd);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(FundManagementViewModel model)
        {
            model.FundTransactions = await _fundRepository.GetTransactionsByDate(model.DateStart, model.DateEnd);
            return View(model);
        }
    }
}
