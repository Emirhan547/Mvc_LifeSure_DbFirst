using System.ComponentModel.DataAnnotations;

namespace Mvc_LifeSure_DbFirst.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Eski şifre gereklidir")]
        [DataType(DataType.Password)]
        [Display(Name = "Eski Şifre")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Yeni şifre gereklidir")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrar")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor")]
        public string ConfirmPassword { get; set; }
    }
}