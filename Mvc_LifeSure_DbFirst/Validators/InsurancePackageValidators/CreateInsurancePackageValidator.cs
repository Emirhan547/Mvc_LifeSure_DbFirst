using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.InsurancePackageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.InsurancePackageValidators
{
    public class CreateInsurancePackageValidator:AbstractValidator<CreateInsurancePackageDto>
    {
        public CreateInsurancePackageValidator()
        {
            RuleFor(x => x.PackageName)
                .NotEmpty().WithMessage("Paket adı boş geçilemez")
                .MaximumLength(100).WithMessage("Paket adı en fazla 100 karakter olabilir");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş geçilemez")
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");

            RuleFor(x => x.BasePrice)
                .NotEmpty().WithMessage("Taban fiyat boş geçilemez")
                .GreaterThan(0).WithMessage("Taban fiyat 0'dan büyük olmalıdır");

            RuleFor(x => x.CoveragePeriodMonths)
                .NotEmpty().WithMessage("Kapsam süresi boş geçilemez")
                .InclusiveBetween(1, 60).WithMessage("Kapsam süresi 1-60 ay arasında olmalıdır");
        }
    }
}