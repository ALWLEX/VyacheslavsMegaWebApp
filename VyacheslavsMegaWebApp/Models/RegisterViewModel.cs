using System.ComponentModel.DataAnnotations;

namespace VyacheslavsMegaWebApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }
    }
}
