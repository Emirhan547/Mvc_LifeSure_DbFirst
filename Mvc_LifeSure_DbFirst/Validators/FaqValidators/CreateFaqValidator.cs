using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.FaqDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.FaqValidators
{
    public class CreateFaqValidator:AbstractValidator<CreateFaqDto>
    {
        public CreateFaqValidator()
        {
            RuleFor(x => x.Answer).NotEmpty().WithMessage("Cevap Boş Bırakılamaz");
            RuleFor(x => x.Question).NotEmpty().WithMessage("Soru Boş Bırakılamaz");
           
        }
    }
}