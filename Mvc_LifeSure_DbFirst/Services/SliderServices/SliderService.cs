using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.SliderDtos;
using Mvc_LifeSure_DbFirst.Repositories.SliderRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Services.SliderServices
{
    public class SliderService:ISliderService
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IValidator<CreateSliderDto> _createValidators;
        private readonly IValidator<UpdateSliderDto> _updateValidators;

        public SliderService(IValidator<CreateSliderDto> createValidators, IValidator<UpdateSliderDto> updateValidators, ISliderRepository sliderRepository)
        {

            _createValidators = createValidators;
            _updateValidators = updateValidators;
            _sliderRepository = sliderRepository;
        }

        public void Create(CreateSliderDto create)
        {
            _createValidators.ValidateAndThrow(create);
            var mappedSliders = create.Adapt<Slider>();
            _sliderRepository.Create(mappedSliders);
        }

        public void Delete(int id)
        {
            var sliders=_sliderRepository.GetById(id);
            if(sliders == null)
            {
                throw new KeyNotFoundException("Slider Not Found");
            }
            _sliderRepository.Delete(sliders);
        }

        public List<ResultSliderDto> GetAll()
        {
            var sliders=_sliderRepository.GetAll();
            return sliders.Adapt<List<ResultSliderDto>>();
        }

        public UpdateSliderDto GetById(int id)
        {
            var sliders=_sliderRepository.GetById(id);
            if( sliders == null)
            {
                throw new KeyNotFoundException("Slider Not Found");
            }
            return sliders.Adapt<UpdateSliderDto>();
        }

        public void Update(UpdateSliderDto update)
        {
            _updateValidators.ValidateAndThrow(update);
            var sliders=_sliderRepository.GetById(update.Id);
            if(sliders ==null)
            {
                throw new KeyNotFoundException("Slider Not Found");
            }
            update.Adapt(sliders);
            _sliderRepository.Update(sliders);
        }
    }
}