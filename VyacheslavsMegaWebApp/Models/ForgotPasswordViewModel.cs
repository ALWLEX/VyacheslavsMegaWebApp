using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VyacheslavsMegaWebApp.Models
{
    public class ForgotPasswordViewModel
    {
        public enum UserInputType { Username, Email, Error }

        [Display(Name = "Имя пользователя или e-mail")]
        public string UserInput { get; set; }

        public UserInputType GetUserInputType()
        {
            if (Regex.IsMatch(UserInput, @"^[A-Za-z0-9_]{3,16}$")) 
                return UserInputType.Username;
            if (Regex.IsMatch(UserInput, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                return UserInputType.Email;
            return UserInputType.Error;
        }
    }
}
