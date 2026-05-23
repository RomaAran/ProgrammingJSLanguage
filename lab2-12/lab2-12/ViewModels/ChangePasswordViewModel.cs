using System.ComponentModel.DataAnnotations;

namespace lab2_12.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{6,}$",
            ErrorMessage = "Минимум 6 символов (латиница и цифры)")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}