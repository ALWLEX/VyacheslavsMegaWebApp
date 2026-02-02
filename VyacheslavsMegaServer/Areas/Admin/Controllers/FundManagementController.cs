using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaServer.Areas.Admin.Models;
using VyacheslavsMegaServer.Data.Entities;
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
            if (!ModelState.IsValid) return View(model);

            switch (model.Filter)
            {
                case FundManagementViewModel.FilterBy.DATE:
                    model.FundTransactions = await _fundRepository.GetTransactionsByDate(model.DateStart, model.DateEnd);
                    break;

                case FundManagementViewModel.FilterBy.LAST:
                    model.FundTransactions = await _fundRepository.GetLastTransaction(model.CountTransactions);
                    break;
            }   
            return View(model);
        }

        public async Task<IActionResult> TransactionDetails(int? transactionId)
        {
            if (transactionId == null) 
                return View(new FundTransaction());
            return View(await _fundRepository.GetTransactionById(transactionId.Value));
        }

        [HttpPost]
        public async Task<IActionResult> TransactionDetails(FundTransaction model)
        {
            if (!ModelState.IsValid) return View(model);
            await _fundRepository.SaveTransaction(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveTransaction(int transactionId)
        {
            await _fundRepository.RemoveTransactionById(transactionId);
            return RedirectToAction(nameof(Index));
        }
    }
}
