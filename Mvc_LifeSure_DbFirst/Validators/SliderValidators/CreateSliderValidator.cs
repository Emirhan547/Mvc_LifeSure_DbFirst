using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.SliderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.SliderValidators
{
    public class CreateSliderValidator:AbstractValidator<CreateSliderDto>
    {
        public CreateSliderValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık Boş Geçilemez");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel Boş Geçilemez");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıkalma Boş Geçilemez");
            RuleFor(x => x.TopTitle).NotEmpty().WithMessage("Üst Başlık Boş Geçilemez");
        }
    }
}