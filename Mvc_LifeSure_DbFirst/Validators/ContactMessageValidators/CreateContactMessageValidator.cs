using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.ContactMessageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.ContactMessageValidators
{
    public class CreateContactMessageValidator : AbstractValidator<CreateContactMessageDto>
    {
        public CreateContactMessageValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim boş geçilemez")
                .MaximumLength(100).WithMessage("İsim en fazla 100 karakter olabilir");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş geçilemez")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz")
                .MaximumLength(100).WithMessage("Email en fazla 100 karakter olabilir");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Mesaj boş geçilemez")
                .MaximumLength(1000).WithMessage("Mesaj en fazla 1000 karakter olabilir")
                .MinimumLength(10).WithMessage("Mesaj en az 10 karakter olmalıdır");
        }
    }
}