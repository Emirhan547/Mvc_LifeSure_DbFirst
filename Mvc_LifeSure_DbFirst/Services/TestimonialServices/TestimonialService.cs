using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Dtos.FeatureDtos;
using Mvc_LifeSure_DbFirst.Dtos.TestimonialDtos;
using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.TestimonialRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Services.TestimonialServices
{
    public class TestimonialService : ITestimonialService
    {
        private readonly ITestimonialRepository _repository;
        private readonly IValidator<CreateTestimonialDto> _createValidators;
        private readonly IValidator<UpdateTestimonialDto> _updateValidators;

        public TestimonialService(ITestimonialRepository repository, IValidator<CreateTestimonialDto> createValidators, IValidator<UpdateTestimonialDto> updateValidators)
        {
            _repository = repository;
            _createValidators = createValidators;
            _updateValidators = updateValidators;
        }

        public void Create(CreateTestimonialDto create)
        {
            _createValidators.ValidateAndThrow(create);
            var mappedTestimonials = create.Adapt<Testimonials>();
            _repository.Create(mappedTestimonials);
        }

        public void Delete(int id)
        {
            var testimonials = _repository.GetById(id);
            if (testimonials == null)
            {
                throw new KeyNotFoundException("Testimonial Not Found");
            }
            _repository.Delete(testimonials);
        }

        public List<ResultTestimonialDto> GetAll()
        {
            var testimonials = _repository.GetAll();
            return testimonials.Adapt<List<ResultTestimonialDto>>();
        }

        public UpdateTestimonialDto GetById(int id)
        {
           var testimonials=_repository.GetById(id);
            if(testimonials == null)
            {
                throw new KeyNotFoundException("Testimonials Not Found");
            }
            return testimonials.Adapt<UpdateTestimonialDto>();
        }

        public void Update(UpdateTestimonialDto update)
        {
            _updateValidators.ValidateAndThrow(update);
            var testimonials=_repository.GetById(update.Id);
            if( testimonials == null)
            {
                throw new KeyNotFoundException("Testimonials Not Found");
            }
            update.Adapt(testimonials);
            _repository.Update(testimonials);
        }
    }
}