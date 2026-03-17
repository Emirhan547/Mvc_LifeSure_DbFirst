using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.AppUserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.AppUserValidators
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş geçilemez");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş geçilemez");
        }
    }
}