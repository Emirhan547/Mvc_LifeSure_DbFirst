using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.BlogDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.BlogValidators
{
    public class CreateBlogValidator:AbstractValidator<CreateBlogDto>
    {
        public CreateBlogValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title boş geçilemez");
            RuleFor(x => x.Author).NotEmpty().WithMessage("Author boş geçilemez");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description boş geçilemez");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("ImageUrl boş geçilemez");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category boş geçilemez");
        }
    }
}