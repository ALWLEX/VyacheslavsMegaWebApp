using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Models;
using VyacheslavsMegaWebApp.Service;

namespace VyacheslavsMegaWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<VmsIdentityUser> _userManager;
        private readonly SignInManager<VmsIdentityUser> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<VmsIdentityUser> userManager, SignInManager<VmsIdentityUser> signInManager, EmailService emailService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            if (!ModelState.IsValid)
                return View(model);

            VmsIdentityUser? user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null)
            {
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неверный логин или пароль");
                return View(model);
            }

            await _signInManager.SignOutAsync();
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(nameof(LoginViewModel.UserName), "Неудачный вход, попробуйте ещё раз");
                return View(model);
            }
            return Redirect(returnUrl ?? "/");
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
                    return RedirectToAction(nameof(SendEmailConfirmationToken));
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
        public async Task<IActionResult> SendEmailConfirmationToken()
        {
            VmsIdentityUser? user = await _userManager.GetUserAsync(User);

            if (user is null)
                return Unauthorized();

            if (user.EmailConfirmedCodeCreationTime is null || 
                DateTime.UtcNow - user.EmailConfirmedCodeCreationTime >= new TimeSpan(0, 3, 0))
            {
                //Get values
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string userId = user.Id;
                string url = Url.Action(nameof(ConfirmEmail), "Account", new { userId, token }, Request.Scheme)!;

                await _emailService.SendEmail(await _emailService.CreateConfirmMessage(user, url));

                //Update email creation time
                user.EmailConfirmedCodeCreationTime = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            TempData["email"] = user.Email;
            return RedirectToAction(nameof(EmailSent));
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            VmsIdentityUser? user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return NotFound();

            IdentityResult confirmationResult = await _userManager.ConfirmEmailAsync(user, token);

            if (confirmationResult.Succeeded)
                return RedirectToAction("Index", "Home");

            return BadRequest();
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            VmsIdentityUser? user;

            switch (model.GetUserInputType())
            {
                case ForgotPasswordViewModel.UserInputType.Username:
                    user = await _userManager.FindByNameAsync(model.UserInput);
                    break;
                case ForgotPasswordViewModel.UserInputType.Email:
                    user = await _userManager.FindByEmailAsync(model.UserInput);
                    break;
                default:
                    ModelState.AddModelError("", "Недопустимый ввод, попробуйте ещё раз");
                    return View(model);
            }

            if (user is null)
            {
                ModelState.AddModelError("", "Пользователь не найден, попробуйте ещё раз");
                return View(model);
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string userId = user.Id;
            string url = Url.Action(nameof(ResetPassword), "Account", new { userId, token }, Request.Scheme);
            await _emailService.SendEmail(await _emailService.CreatePasswordResetMessage(user, url));
            TempData["email"] = user.Email;
            return RedirectToAction(nameof(EmailSent));
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string userId, string token)
        {
            return View(new ResetPasswordViewModel { UserId = userId, Token = token });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            VmsIdentityUser user = await _userManager.FindByIdAsync(model.UserId);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            ModelState.AddModelError("", "Не удалось сбросить пароль, попробуйте ещё раз");
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult EmailSent()
        {
            return View();
        }
    }
}
