using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.PolicyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.PolicyValidators
{
    public class UpdatePolicyValidator:AbstractValidator<UpdatePolicyDto>
    {
        public UpdatePolicyValidator()
        {
            RuleFor(x => x.PolicyNumber)
               .NotEmpty().WithMessage("Poliçe numarası boş geçilemez")
               .MaximumLength(20).WithMessage("Poliçe numarası en fazla 20 karakter olabilir");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Başlangıç tarihi boş geçilemez");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Bitiş tarihi boş geçilemez")
                .GreaterThan(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır");

            RuleFor(x => x.PremiumAmount)
                .NotEmpty().WithMessage("Prim tutarı boş geçilemez")
                .GreaterThan(0).WithMessage("Prim tutarı 0'dan büyük olmalıdır");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Kullanıcı seçimi zorunludur");

            RuleFor(x => x.InsurancePackageId)
                .NotEmpty().WithMessage("Sigorta paketi seçimi zorunludur")
                .GreaterThan(0).WithMessage("Geçerli bir paket seçiniz");
        }
    }
}