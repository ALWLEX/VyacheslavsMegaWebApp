using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Data.Repositories;
using VyacheslavsMegaWebApp.Models;
using VyacheslavsMegaWebApp.Service;

namespace VyacheslavsMegaWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<VmsIdentityUser> _userManager;
        private readonly SignInManager<VmsIdentityUser> _signInManager;
        private readonly IdentityRepository _identityRepository;
        private readonly EmailService _emailService;

        public AccountController(UserManager<VmsIdentityUser> userManager, SignInManager<VmsIdentityUser> signInManager, IdentityRepository identityRepository, EmailService emailService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityRepository = identityRepository;
            _emailService = emailService;
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? "/";
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                VmsIdentityUser? user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null) 
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (signInResult.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неверный логин или пароль");
            }
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                VmsIdentityUser user = new VmsIdentityUser { Email = model.Email, UserName = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction(nameof(ConfirmEmail));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult AccountSettings()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                VmsIdentityUser currentUser = await _userManager.GetUserAsync(User);
                await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.NewPassword);
                return RedirectToAction(nameof(AccountSettings));
            }
            
            return View(nameof(AccountSettings), model);
        }

        [Authorize]
        public async Task<IActionResult> ConfirmEmail()
        {
            VmsIdentityUser user = await _userManager.GetUserAsync(User);
            if (user.EmailConfirmedCodeCreationTime is null || DateTime.UtcNow - user.EmailConfirmedCodeCreationTime >= new TimeSpan(0, 3, 0))
                await _emailService.SendEmail(await _emailService.CreateConfirmMessage(user, await _identityRepository.GetEmailConfirmationCode(user.Id)));
            return View(new EmailConfirmationViewModel() { User = user });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ConfirmationCode = string.Empty;
                return View(model);
            }

            VmsIdentityUser user = await _userManager.GetUserAsync(User);
            IdentityRepository.EmailConfirmationResult result = await _identityRepository.ConfirmEmail(user.Id, model.ConfirmationCode);
            if ( result == IdentityRepository.EmailConfirmationResult.Success) 
                return RedirectToAction("Index", "Home");

            string errorMessage = result == IdentityRepository.EmailConfirmationResult.Timeout
                ? "Время действия кода вышло, запросите новый код"
                : "Неверный код, попробуйте ещё раз";
            model.ConfirmationCode = string.Empty;
            ModelState.AddModelError(string.Empty, errorMessage);
            return View(model);
        }
    }
}
