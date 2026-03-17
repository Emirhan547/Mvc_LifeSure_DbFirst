using System.ComponentModel.DataAnnotations;

namespace Mvc_LifeSure_DbFirst.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}