using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.TestimonialDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.TestimonialValidators
{
    public class CreateTestimonialValidator:AbstractValidator<CreateTestimonialDto>
    {
        public CreateTestimonialValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim Boş Geçilemez");
            RuleFor(x => x.Profession).NotEmpty().WithMessage("Profession Boş Geçilemez");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Görsel Boş Geçilemez");
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Yorum Boş Geçilemez");
            RuleFor(x => x.Rating).NotEmpty().WithMessage("Rating Boş Geçilemez");
        }
    }
}