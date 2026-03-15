using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.ServiceDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.ServiceValidators
{
    public class UpdateServicesValidator:AbstractValidator<UpdateServicesDto>
    {
        public UpdateServicesValidator()
        {
            RuleFor(x => x.Icon).NotEmpty().WithMessage("Icon Boş Bırakılamaz");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Başlık Boş Bırakılamaz");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel Boş Bırakılamaz");
        }
    }
}