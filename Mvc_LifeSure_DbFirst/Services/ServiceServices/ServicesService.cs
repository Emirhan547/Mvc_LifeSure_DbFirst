using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Dtos.ServiceDtos;
using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.ServiceRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Services.ServiceServices
{
    public class ServicesService : IServicesService
    {
        private readonly IServicesRepository _serviceRepository;
        private readonly IValidator<CreateServicesDto> _createValidator;
        private readonly IValidator<UpdateServicesDto> _updateValidator;

        public ServicesService(IServicesRepository serviceRepository, IValidator<CreateServicesDto> createValidator, IValidator<UpdateServicesDto> updateValidator)
        {
            _serviceRepository = serviceRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public void Create(CreateServicesDto create)
        {
            _createValidator.ValidateAndThrow(create);
            var mappedService = create.Adapt<Models.Services>();
            _serviceRepository.Create(mappedService);
        }

        public void Delete(int id)
        {
            var services = _serviceRepository.GetById(id);
            if(services == null)
            {
                throw new KeyNotFoundException("Service Not Found");
            }
            _serviceRepository.Delete(services);
        }

        public List<ResultServicesDto> GetAll()
        {
            var services=_serviceRepository.GetAll();
            return services.Adapt<List<ResultServicesDto>>();
        }

        public UpdateServicesDto GetById(int id)
        {
            var services= _serviceRepository.GetById(id);
            if( services == null)
            {
                throw new KeyNotFoundException("Service Not Found");
            }
            return services.Adapt<UpdateServicesDto>();
        }

        public void Update(UpdateServicesDto update)
        {
            _updateValidator.ValidateAndThrow(update);
            var services=_serviceRepository.GetById(update.Id);
            if(services==null)
            {
                throw new KeyNotFoundException("Service Not Found");
            }
            update.Adapt(services);
            _serviceRepository.Update(services);
        }
    }
}