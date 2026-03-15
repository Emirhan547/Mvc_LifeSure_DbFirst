using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.AboutDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.AboutValidators
{
    public class CreateAboutValidator:AbstractValidator<CreateAboutDto>
    {
        public CreateAboutValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title boş geçilemez");
            RuleFor(x => x.Description1).NotEmpty().WithMessage("Description1 boş geçilemez");
            RuleFor(x => x.Description2).NotEmpty().WithMessage("Description2 boş geçilemez");
            RuleFor(x => x.SubTitle).NotEmpty().WithMessage("SubTitle boş geçilemez");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl boş geçilemez");
        }
    }
}