using System.ComponentModel.DataAnnotations;

namespace lab2_12.Models.ViewModels
{
    public class RegisterViewModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}