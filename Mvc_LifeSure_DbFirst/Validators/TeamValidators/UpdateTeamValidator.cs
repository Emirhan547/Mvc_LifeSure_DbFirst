using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.TeamDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Validators.TeamValidators
{
    public class UpdateTeamValidator:AbstractValidator<UpdateTeamDto>
    {
        public UpdateTeamValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim boş bırakılamaz");
            RuleFor(x => x.Profession).NotEmpty().WithMessage("Profession boş bırakılamaz");
            RuleFor(x => x.TwitterUrl).NotEmpty().WithMessage("TwitterUrl boş bırakılamaz");
            RuleFor(x => x.InstagramUrl).NotEmpty().WithMessage("InstagramUrl boş bırakılamaz");
            RuleFor(x => x.FacebookUrl).NotEmpty().WithMessage("FaceBookUrl boş bırakılamaz");
        }
    }
}