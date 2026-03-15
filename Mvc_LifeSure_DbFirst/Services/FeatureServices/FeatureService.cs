using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Dtos.FeatureDtos;
using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.FeatureRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Services.FeatureServices
{
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly IValidator<CreateFeatureDto> _createValidator;
        private readonly IValidator<UpdateFeatureDto> _updateValidator;

        public FeatureService(IFeatureRepository featureRepository, IValidator<CreateFeatureDto> createValidator, IValidator<UpdateFeatureDto> updateValidator)
        {
            _featureRepository = featureRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public void Create(CreateFeatureDto createFeatureDto)
        {
            _createValidator.ValidateAndThrow(createFeatureDto);
            var mappedFeatures = createFeatureDto.Adapt<Features>();
            _featureRepository.Create(mappedFeatures);
        }

        public void Delete(int id)
        {
          var features=_featureRepository.GetById(id);
            if (features == null)
            {
                throw new KeyNotFoundException("Features Not Found");
            }
            _featureRepository.Delete(features);
        }

        public List<ResultFeatureDto> GetAll()
        {
           var features=_featureRepository.GetAll();
            return features.Adapt<List<ResultFeatureDto>>();
        }

        public UpdateFeatureDto GetById(int id)
        {
            var features = _featureRepository.GetById(id);
            if(features == null)
            {
                throw new KeyNotFoundException("Features Not Found");
            }
            return features.Adapt<UpdateFeatureDto>();
        }

        public void Update(UpdateFeatureDto updateFeatureDto)
        {
            _updateValidator.ValidateAndThrow(updateFeatureDto);
            var features=_featureRepository.GetById(updateFeatureDto.Id);
            if( features == null)
            {
                throw new KeyNotFoundException("Features Not Found");
            }
            updateFeatureDto.Adapt(features);
            _featureRepository.Update(features);
        }
    }
}