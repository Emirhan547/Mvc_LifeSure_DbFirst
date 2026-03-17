using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.InsurancePackageDtos;
using Mvc_LifeSure_DbFirst.Repositories.InsurancePackageRepositories;
using Mvc_LifeSure_DbFirst.Repositories.PolicyRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.InsurancePackageServices
{
    public class InsurancePackageService : IInsurancePackageService
    {
        private readonly IInsurancePackageRepository _packageRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IValidator<CreateInsurancePackageDto> _createValidator;
        private readonly IValidator<UpdateInsurancePackageDto> _updateValidator;

        public InsurancePackageService(
            IInsurancePackageRepository packageRepository,
            IPolicyRepository policyRepository,
            IValidator<CreateInsurancePackageDto> createValidator,
            IValidator<UpdateInsurancePackageDto> updateValidator)
        {
            _packageRepository = packageRepository;
            _policyRepository = policyRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public List<ResultInsurancePackageDto> GetAll()
        {
            var packages = _packageRepository.GetAll();
            var result = new List<ResultInsurancePackageDto>();

            foreach (var package in packages)
            {
                var packageDto = package.Adapt<ResultInsurancePackageDto>();
                packageDto.PolicyCount = _policyRepository.GetPoliciesByPackage(package.Id).Count;
                result.Add(packageDto);
            }

            return result;
        }

        public ResultInsurancePackageDto GetById(int id)
        {
            var package = _packageRepository.GetById(id);
            if (package == null)
                throw new KeyNotFoundException("Sigorta paketi bulunamadı");

            var packageDto = package.Adapt<ResultInsurancePackageDto>();
            packageDto.PolicyCount = _policyRepository.GetPoliciesByPackage(id).Count;

            return packageDto;
        }

        public ResultInsurancePackageDto GetPackageWithPolicies(int id)
        {
            var package = _packageRepository.GetPackageWithPolicies(id);
            if (package == null)
                throw new KeyNotFoundException("Sigorta paketi bulunamadı");

            var packageDto = package.Adapt<ResultInsurancePackageDto>();
            packageDto.PolicyCount = package.Policies?.Count ?? 0;

            return packageDto;
        }

        public List<ResultInsurancePackageDto> GetActivePackages()
        {
            var packages = _packageRepository.GetActivePackages();
            var result = new List<ResultInsurancePackageDto>();

            foreach (var package in packages)
            {
                var packageDto = package.Adapt<ResultInsurancePackageDto>();
                packageDto.PolicyCount = _policyRepository.GetPoliciesByPackage(package.Id).Count;
                result.Add(packageDto);
            }

            return result;
        }

        public void Create(CreateInsurancePackageDto createDto)
        {
            _createValidator.ValidateAndThrow(createDto);

            var package = createDto.Adapt<InsurancePackage>();
            _packageRepository.Create(package);
        }

        public void Update(UpdateInsurancePackageDto updateDto)
        {
            _updateValidator.ValidateAndThrow(updateDto);

            var package = _packageRepository.GetById(updateDto.Id);
            if (package == null)
                throw new KeyNotFoundException("Sigorta paketi bulunamadı");

            updateDto.Adapt(package);
            _packageRepository.Update(package);
        }

        public void Delete(int id)
        {
            var package = _packageRepository.GetById(id);
            if (package == null)
                throw new KeyNotFoundException("Sigorta paketi bulunamadı");

            // Pakete ait poliçe var mı kontrol et
            var policies = _policyRepository.GetPoliciesByPackage(id);
            if (policies.Any())
                throw new InvalidOperationException("Bu pakete ait poliçeler bulunmaktadır. Önce poliçeleri silin.");

            _packageRepository.Delete(package);
        }

        public void TogglePackageStatus(int id)
        {
            var package = _packageRepository.GetById(id);
            if (package == null)
                throw new KeyNotFoundException("Sigorta paketi bulunamadı");

            package.IsActive = !package.IsActive;
            _packageRepository.Update(package);
        }
    }
}