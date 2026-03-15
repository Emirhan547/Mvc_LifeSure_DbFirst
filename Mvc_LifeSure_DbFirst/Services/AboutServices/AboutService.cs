using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Dtos.AboutDtos;
using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.AboutRepositories;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Services.AboutServices
{
    public class AboutService : IAboutService
    {
        private readonly IAboutRepository _aboutRepository;
        private readonly IValidator<CreateAboutDto> _createValidator;
        private readonly IValidator<UpdateAboutDto> _updateValidator;
        public AboutService(IAboutRepository aboutRepository, IValidator<CreateAboutDto> createValidator, IValidator<UpdateAboutDto> updateValidator)
        {
            _aboutRepository = aboutRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public void Create(CreateAboutDto create)
        {
            _createValidator.ValidateAndThrow(create);
            var aboutsMapped=create.Adapt<Abouts>();
            _aboutRepository.Create(aboutsMapped);
           
        }

        public void Delete(int id)
        {
            var abouts=_aboutRepository.GetById(id);
            if(abouts == null)
            {
                throw new KeyNotFoundException("About Not Found");
            }
            _aboutRepository.Delete(abouts);
        }

        public List<ResultAboutDto> GetAll()
        {
            var abouts = _aboutRepository.GetAll();
            return abouts.Adapt<List<ResultAboutDto>>();
        }

        public UpdateAboutDto GetById(int id)
        {
            var abouts=_aboutRepository.GetById(id);
            if(abouts == null)
            {
               throw new KeyNotFoundException("About Not Found");
            }
            return abouts.Adapt<UpdateAboutDto>();
        }

        public void Update(UpdateAboutDto update)
        {
            _updateValidator.ValidateAndThrow(update);
            var about = _aboutRepository.GetById(update.Id);
            if(about == null)
            {
                throw new KeyNotFoundException("About Not Found");
            }
            update.Adapt(about);
           

            _aboutRepository.Update(about);
        }
    }
}