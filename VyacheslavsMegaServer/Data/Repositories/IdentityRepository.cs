using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using VyacheslavsMegaServer.Areas.Admin.Models;

namespace VyacheslavsMegaServer.Data.Repositories
{
    public class IdentityRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public const string NO_ROLE = "None";
        public const string ADMIN_ROLE = "Admin";

        public IdentityRepository(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) 
        { 
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<UserViewModel>> GetAllUsers() 
        {
            List<UserViewModel> users = new List<UserViewModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                string userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                users.Add(new UserViewModel
                {
                    IdentityUserInfo = user,
                    Role = string.IsNullOrEmpty(userRole) ? NO_ROLE : userRole
                }) ;
            }
            return users;
        }
        public async Task<List<SelectListItem>> GetRolesSelectListToFilter()
        {
            List<SelectListItem> availableRoleNames =
            [
                new SelectListItem(NO_ROLE, NO_ROLE, true),
                .. await _roleManager.Roles
                    .Where(r => r.Name != null)
                    .Select(r => new SelectListItem(r.Name, r.Name))
                    .ToListAsync(),
            ];
            return availableRoleNames;
        }

        public async Task<List<SelectListItem>> GetRolesSelectListToSet()
        {
            List<SelectListItem> availableRoleNames =
            [
                new SelectListItem(NO_ROLE, NO_ROLE, true),
                .. await _roleManager.Roles
                    .Where(r => r.Name != null && r.Name != ADMIN_ROLE)
                    .Select(r => new SelectListItem(r.Name, r.Name))
                    .ToListAsync(),
            ];
            return availableRoleNames;
        }

        public async Task<List<string>> GetRoleNames()
        {
            return await _roleManager.Roles
                    .Where(r => r.Name != null)
                    .Select(r => r.Name)
                    .ToListAsync();
        }

        public async Task RemoveUser(string userId)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }

        public async Task<UserViewModel> GetUserById(string userId)
        {
            IdentityUser identityUser = await _userManager.FindByIdAsync(userId);
            string userRole = (await _userManager.GetRolesAsync(identityUser)).FirstOrDefault();
            return new UserViewModel()
            {
                IdentityUserInfo = identityUser,
                Role = string.IsNullOrEmpty(userRole) ? NO_ROLE : userRole
            };
        }

        public async Task ApplyRoleToUser(string userId, string role)
        {
            IdentityUser identityUser = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRolesAsync(identityUser, await GetRoleNames());
            if (role != NO_ROLE) 
            {
                await _userManager.AddToRoleAsync(identityUser, role);
                return;
            }
        }
    }
}
