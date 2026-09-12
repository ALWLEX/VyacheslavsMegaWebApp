using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<AccountController> _localizer;

        #region String constants

        private const string UserNotFound = "UserNotFound";
        private const string UserNameNotFound = "UserNameNotFound";
        private const string UserIsLockedOut = "UserIsLockedOut";
        private const string UserIsNotAllowed = "UserIsNotAllowed";
        private const string NotAuthorized = "NotAuthorized";
        private const string WrongInput = "WrongInput";
        private const string WrongLoginOrPassword = "WrongLoginOrPassword";

        #endregion

        public AccountController(UserManager<VmsIdentityUser> userManager, SignInManager<VmsIdentityUser> signInManager, EmailService emailService, IStringLocalizer<AccountController> localizer) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _localizer = localizer;
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
                ModelState.AddModelError(UserNameNotFound, string.Format(_localizer[UserNameNotFound], model.UserName));
                return View(model);
            }

            await _signInManager.SignOutAsync();
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl ?? "/");
            }

            bool wrongParrwordCheck = !(signInResult.IsLockedOut || signInResult.IsNotAllowed);
            if (wrongParrwordCheck)
            {
                ModelState.AddModelError(WrongLoginOrPassword, _localizer[WrongLoginOrPassword]);
            }

            if (signInResult.IsLockedOut)
                ModelState.AddModelError(UserIsLockedOut, string.Format(_localizer[UserIsLockedOut], model.UserName));
            if (signInResult.IsNotAllowed)
                ModelState.AddModelError(UserIsNotAllowed, string.Format(_localizer[UserIsNotAllowed], model.UserName));
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
            if (!ModelState.IsValid)
                return View(model);

            VmsIdentityUser user = new VmsIdentityUser { Email = model.Email, UserName = model.UserName };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // установка куки
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction(nameof(SendEmailConfirmationToken));
            }

            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
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
                return View(nameof(AccountSettings), model);

            VmsIdentityUser? user = await _userManager.GetUserAsync(User);

            if (user is null)
                return Unauthorized(_localizer[NotAuthorized]);

            IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
                return RedirectToAction(nameof(AccountSettings));
            
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return View(nameof(AccountSettings), model);
        }

        [Authorize]
        public async Task<IActionResult> SendEmailConfirmationToken()
        {
            VmsIdentityUser? user = await _userManager.GetUserAsync(User);

            if (user is null)
                return Unauthorized(_localizer[NotAuthorized]);

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
                    ModelState.AddModelError(WrongInput, string.Format(_localizer[WrongInput], model.UserInput));
                    return View(model);
            }

            if (user is null)
            {
                ModelState.AddModelError(UserNotFound, _localizer[UserNotFound]);
                return View(model);
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string userId = user.Id;
            string url = Url.Action(nameof(ResetPassword), "Account", new { userId, token }, Request.Scheme)!;
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
            VmsIdentityUser? user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
                return NotFound(_localizer[UserNotFound]);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult EmailSent()
        {
            return View();
        }
    }
}
