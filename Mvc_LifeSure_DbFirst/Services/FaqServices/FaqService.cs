using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Dtos.FaqDtos;
using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.FaqRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Services.FaqServices
{
    public class FaqService : IFaqService
    {
        private readonly IFaqRepository _faqRepository;
        private readonly IValidator<CreateFaqDto> _createValidator;
        private readonly IValidator<UpdateFaqDto> _updateValidator;
        public FaqService(IFaqRepository faqRepository, IValidator<CreateFaqDto> createValidator, IValidator<UpdateFaqDto> updateValidator)
        {
            _faqRepository = faqRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }


        public void Create(CreateFaqDto create)
        {
            _createValidator.ValidateAndThrow(create);
            var mappedFaqs = create.Adapt<Faqs>();
            _faqRepository.Create(mappedFaqs);
        }

        public void Delete(int id)
        {
            var faqs=_faqRepository.GetById(id);
            if (faqs == null)
            {
                throw new KeyNotFoundException("Faq Not Found");
            }
            _faqRepository.Delete(faqs);

        }

        public List<ResultFaqDto> GetAll()
        {
            var faqs=_faqRepository.GetAll();
            return faqs.Adapt<List<ResultFaqDto>>();
        }

        public UpdateFaqDto GetById(int id)
        {
            var faqs=_faqRepository.GetById(id);
            if(faqs == null)
            {
                throw new KeyNotFoundException("Faq Not Found");
            }
            return faqs.Adapt<UpdateFaqDto>();
        }

        public void Update(UpdateFaqDto update)
        {
           _updateValidator.ValidateAndThrow(update);
            var faqs=_faqRepository.GetById(update.Id);
            if( faqs == null)
            {
                throw new KeyNotFoundException("Faqs Not Found");
            }
            update.Adapt(faqs);
            _faqRepository.Update(faqs);

        }
    }
}