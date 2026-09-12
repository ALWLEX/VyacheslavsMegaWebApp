using Microsoft.AspNetCore.Identity;
using VyacheslavsMegaWebApp.Data.Entities.Identity;
using VyacheslavsMegaWebApp.Data.JSON.Configuration;

namespace VyacheslavsMegaWebApp.Service
{
    public class VmsUserValidator : UserValidator<VmsIdentityUser>
    {
        private readonly VmsIdentityErrorDescriber _errorDescriber;

        public VmsUserValidator(IdentityErrorDescriber errorDescriber)
        {
            _errorDescriber = errorDescriber as VmsIdentityErrorDescriber ?? throw new InvalidOperationException("Зарегистрирован неверный IdentityErrorDescriber.");
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<VmsIdentityUser> manager, VmsIdentityUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            string userEmailDomain = user.Email!.Substring(user.Email.IndexOf('@') + 1).ToLower();
            if (!ProjectConfig.AcceptedEmailDomains.Contains(userEmailDomain))
            {
                errors.Add(_errorDescriber.UnallowedEmailDomain(userEmailDomain));
            }

            return await Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
