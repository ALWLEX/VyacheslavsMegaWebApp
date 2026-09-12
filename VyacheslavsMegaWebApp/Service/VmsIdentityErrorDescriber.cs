using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace VyacheslavsMegaWebApp.Service
{
    public class IdentityErrors { }

    public class VmsIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<IdentityErrors> _localizer;

        public VmsIdentityErrorDescriber(IStringLocalizer<IdentityErrors> localizer)
        {
            _localizer = localizer;
        }

        private string GetString(string name, params object[] args)
        {
            var localizedString = _localizer[name];

            return localizedString.ResourceNotFound
                ? name
                : string.Format(localizedString.Value, args);
        }

        public override IdentityError DefaultError()
            => new() { Code = nameof(DefaultError), Description = GetString(nameof(DefaultError)) };

        public override IdentityError ConcurrencyFailure()
            => new() { Code = nameof(ConcurrencyFailure), Description = GetString(nameof(ConcurrencyFailure)) };

        public override IdentityError PasswordMismatch()
            => new() { Code = nameof(PasswordMismatch), Description = GetString(nameof(PasswordMismatch)) };

        public override IdentityError InvalidToken()
            => new() { Code = nameof(InvalidToken), Description = GetString(nameof(InvalidToken)) };

        public override IdentityError LoginAlreadyAssociated()
            => new() { Code = nameof(LoginAlreadyAssociated), Description = GetString(nameof(LoginAlreadyAssociated)) };

        public override IdentityError InvalidUserName(string? userName)
            => new() { Code = nameof(InvalidUserName), Description = GetString(nameof(InvalidUserName), userName!) };

        public override IdentityError InvalidEmail(string? email)
            => new() { Code = nameof(InvalidEmail), Description = GetString(nameof(InvalidEmail), email!) };

        public override IdentityError DuplicateUserName(string userName)
            => new() { Code = nameof(DuplicateUserName), Description = GetString(nameof(DuplicateUserName), userName) };

        public override IdentityError DuplicateEmail(string email)
            => new() { Code = nameof(DuplicateEmail), Description = GetString(nameof(DuplicateEmail), email) };

        public override IdentityError InvalidRoleName(string? role)
            => new() { Code = nameof(InvalidRoleName), Description = GetString(nameof(InvalidRoleName), role!) };

        public override IdentityError DuplicateRoleName(string role)
            => new() { Code = nameof(DuplicateRoleName), Description = GetString(nameof(DuplicateRoleName), role) };

        public override IdentityError UserAlreadyHasPassword()
            => new() { Code = nameof(UserAlreadyHasPassword), Description = GetString(nameof(UserAlreadyHasPassword)) };

        public override IdentityError UserLockoutNotEnabled()
            => new() { Code = nameof(UserLockoutNotEnabled), Description = GetString(nameof(UserLockoutNotEnabled)) };

        public override IdentityError UserAlreadyInRole(string role)
            => new() { Code = nameof(UserAlreadyInRole), Description = GetString(nameof(UserAlreadyInRole), role) };

        public override IdentityError UserNotInRole(string role)
            => new() { Code = nameof(UserNotInRole), Description = GetString(nameof(UserNotInRole), role) };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new() { Code = nameof(PasswordRequiresNonAlphanumeric), Description = GetString(nameof(PasswordRequiresNonAlphanumeric)) };

        public override IdentityError PasswordRequiresDigit()
            => new() { Code = nameof(PasswordRequiresDigit), Description = GetString(nameof(PasswordRequiresDigit)) };

        public override IdentityError PasswordRequiresLower()
            => new() { Code = nameof(PasswordRequiresLower), Description = GetString(nameof(PasswordRequiresLower)) };

        public override IdentityError PasswordRequiresUpper()
            => new() { Code = nameof(PasswordRequiresUpper), Description = GetString(nameof(PasswordRequiresUpper)) };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => new() { Code = nameof(PasswordRequiresUniqueChars), Description = GetString(nameof(PasswordRequiresUniqueChars), uniqueChars) };

        public override IdentityError PasswordTooShort(int length)
            => new() { Code = nameof(PasswordTooShort), Description = GetString(nameof(PasswordTooShort), length) };
    }
}
