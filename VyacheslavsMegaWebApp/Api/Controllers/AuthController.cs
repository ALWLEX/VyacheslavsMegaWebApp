using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using VyacheslavsMegaWebApp.Api.Models;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Data.Repositories;

namespace VyacheslavsMegaWebApp.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<VmsIdentityUser> _userManager;
        private readonly LauncherSessionRepository _sessionRepository;
        private readonly IStringLocalizer<AuthController> _localizer;

        public AuthController(UserManager<VmsIdentityUser> userManager, LauncherSessionRepository sessionRepository, IStringLocalizer<AuthController> localizer)
        {
            _userManager = userManager;
            _sessionRepository = sessionRepository;
            _localizer = localizer;
        }

        [HttpPost(nameof(LoginWithUsername))]
        public async Task<IActionResult> LoginWithUsername(LoginWithUsernameRequest request)
        {
            VmsIdentityUser? user = await _userManager.FindByNameAsync(request.Username);
            if (user is null)
                return Unauthorized(new LoginResponse 
                {
                    Succeeded = false,
                    Error = string.Format(_localizer["UserNotFound"], request.Username)
                });
            if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password) == PasswordVerificationResult.Failed)
                return Unauthorized(new LoginResponse
                {
                    Succeeded = false,
                    Error = _localizer["WrongPassword"]
                });
            if (!user.EmailConfirmed)
                return Forbid();

            return Ok(new LoginResponse 
            { 
                Succeeded = true,
                SessionKey = await _sessionRepository.CreateSessionKey(user)
            });
        }

        [HttpPost(nameof(LoginWithSessionKey))]
        public IActionResult LoginWithSessionKey(LoginWithSessionKeyRequest request)
        {
            return Ok();
        }
    }
}
