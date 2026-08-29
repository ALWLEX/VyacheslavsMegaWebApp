using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using VyacheslavsMegaWebApp.Areas.Admin.Models;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using System.Security.Cryptography;
using System.Runtime.Intrinsics.Arm;

namespace VyacheslavsMegaWebApp.Data.Repositories
{
    public class IdentityRepository
    {
        private readonly UserManager<VmsIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public const string NO_ROLE = "None";
        public const string ADMIN_ROLE = "Admin";

        public enum EmailConfirmationResult { Success, WrongCode, Timeout }

        public IdentityRepository(UserManager<VmsIdentityUser> userManager, RoleManager<IdentityRole> roleManager) 
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
            VmsIdentityUser? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }

        public async Task<UserViewModel> GetUserById(string userId)
        {
            VmsIdentityUser identityUser = await _userManager.FindByIdAsync(userId);
            string userRole = (await _userManager.GetRolesAsync(identityUser)).FirstOrDefault();
            return new UserViewModel()
            {
                IdentityUserInfo = identityUser,
                Role = string.IsNullOrEmpty(userRole) ? NO_ROLE : userRole
            };
        }

        public async Task ApplyRoleToUser(string userId, string role)
        {
            VmsIdentityUser identityUser = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRolesAsync(identityUser, await GetRoleNames());
            if (role != NO_ROLE) 
            {
                await _userManager.AddToRoleAsync(identityUser, role);
                return;
            }
        }

        public async Task<bool> GetEmailConfirmed(string userId)
        {
            VmsIdentityUser? user = await _userManager.FindByIdAsync(userId);
            return user.EmailConfirmed;
        }

        public async Task<EmailConfirmationResult> ConfirmEmail(string userId, string confirmationCode)
        {
            VmsIdentityUser? user = await _userManager.FindByIdAsync(userId);
            
            //Check timeout
            if (DateTime.UtcNow - user.EmailConfirmedCodeCreationTime > new TimeSpan(0, 15, 0)) 
                return EmailConfirmationResult.Timeout;

            //Check code
            byte[] codeHash = await SHA256.Create().ComputeHashAsync(new MemoryStream(BitConverter.GetBytes(int.Parse(confirmationCode))));

            if (!user.EmailConfirmCodeHash.SequenceEqual(codeHash))
                return EmailConfirmationResult.WrongCode;
            
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            return EmailConfirmationResult.Success;
        }

        public async Task SetEmailConfirmed(string userId, bool confirmed)
        {
            VmsIdentityUser? user = await _userManager.FindByIdAsync(userId);
            user.EmailConfirmed = confirmed;
            await _userManager.UpdateAsync(user);
            
        }

        public async Task<string> GetEmailConfirmationCode(string userId)
        {
            VmsIdentityUser? user = await _userManager.FindByIdAsync(userId);
            int code = RandomNumberGenerator.GetInt32(100000, 1000000);
            
            user.EmailConfirmCodeHash = await SHA256.Create().ComputeHashAsync(new MemoryStream(BitConverter.GetBytes(code)));
            user.EmailConfirmedCodeCreationTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return code.ToString();
        }
    }
}
