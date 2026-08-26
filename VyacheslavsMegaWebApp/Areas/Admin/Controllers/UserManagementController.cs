using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Areas.Admin.Models;
using VyacheslavsMegaWebApp.Data.Repositories;

namespace VyacheslavsMegaWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagementController : Controller
    {
        private readonly IdentityRepository _identityRepository;

        public UserManagementController(IdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<IActionResult> Index()
        { 
            return View(new UserManagerViewModel()
            {
                Users = await _identityRepository.GetAllUsers(),
                RolesSelectListToFilter = await _identityRepository.GetRolesSelectListToFilter(),
                RolesSelectListToSet = await _identityRepository.GetRolesSelectListToSet()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Index(UserManagerViewModel model)
        {
            model.Users = await _identityRepository.GetAllUsers();
            model.RolesSelectListToFilter = await _identityRepository.GetRolesSelectListToFilter();
            model.RolesSelectListToSet = await _identityRepository.GetRolesSelectListToSet();
            return View(model);
        }

        public async Task<IActionResult> RemoveUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId)) 
            {
                await _identityRepository.RemoveUser(userId);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserRoles(UserManagerViewModel model) 
        {
            List<UserViewModel> usersToUpdate = model.Users
                .Where(u => u.Role != null)
                .ToList();

            foreach (UserViewModel user in usersToUpdate) 
            {
                await _identityRepository.ApplyRoleToUser(user.IdentityUserInfo.Id, user.Role);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
