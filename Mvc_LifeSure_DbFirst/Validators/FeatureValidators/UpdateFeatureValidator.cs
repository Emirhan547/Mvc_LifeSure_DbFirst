using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.FeatureDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.FeatureValidators
{
    public class UpdateFeatureValidator:AbstractValidator<UpdateFeatureDto>
    {
        public UpdateFeatureValidator()
        {
            RuleFor(x => x.Icon).NotEmpty().WithMessage("Icon boş geçilemez");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık boş geçilemez");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş geçilemez");

        }
    }
}