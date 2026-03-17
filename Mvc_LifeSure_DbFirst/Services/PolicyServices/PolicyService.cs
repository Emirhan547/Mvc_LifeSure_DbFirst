using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.PolicyDtos;
using Mvc_LifeSure_DbFirst.Repositories.PolicyRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.PolicyServices
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IValidator<CreatePolicyDto> _createValidator;
        private readonly IValidator<UpdatePolicyDto> _updateValidator;

        public PolicyService(
            IPolicyRepository policyRepository,
            IValidator<CreatePolicyDto> createValidator,
            IValidator<UpdatePolicyDto> updateValidator)
        {
            _policyRepository = policyRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public List<ResultPolicyDto> GetAll()
        {
            var policies = _policyRepository.GetAll();
            var result = new List<ResultPolicyDto>();

            foreach (var policy in policies)
            {
                var policyWithDetails = _policyRepository.GetPolicyWithDetails(policy.Id);
                if (policyWithDetails != null)
                    result.Add(MapToResultDto(policyWithDetails));
            }

            return result;
        }

        public List<ResultPolicySimpleDto> GetAllSimple()
        {
            var policies = _policyRepository.GetAll();
            var result = new List<ResultPolicySimpleDto>();

            foreach (var policy in policies)
            {
                var policyWithDetails = _policyRepository.GetPolicyWithDetails(policy.Id);
                if (policyWithDetails != null)
                {
                    result.Add(new ResultPolicySimpleDto
                    {
                        Id = policy.Id,
                        PolicyNumber = policy.PolicyNumber,
                        StartDate = policy.StartDate,
                        PremiumAmount = policy.PremiumAmount,
                        UserFullName = policyWithDetails.User != null
                            ? $"{policyWithDetails.User.FirstName} {policyWithDetails.User.LastName}"
                            : "Belirtilmemiş",
                        PackageName = policyWithDetails.InsurancePackage?.PackageName
                    });
                }
            }

            return result;
        }

        public ResultPolicyDto GetById(int id)
        {
            var policy = _policyRepository.GetPolicyWithDetails(id);
            if (policy == null)
                throw new KeyNotFoundException("Poliçe bulunamadı");

            return MapToResultDto(policy);
        }

        public ResultPolicyDto GetPolicyWithDetails(int id)
        {
            return GetById(id);
        }

        public List<ResultPolicyDto> GetPoliciesByUser(string userId)
        {
            var policies = _policyRepository.GetPoliciesByUser(userId);  // Direkt string kullan
            var result = new List<ResultPolicyDto>();

            foreach (var policy in policies)
            {
                result.Add(MapToResultDto(policy));
            }

            return result;
        }

        public List<ResultPolicyDto> GetPoliciesByPackage(int packageId)
        {
            var policies = _policyRepository.GetPoliciesByPackage(packageId);
            var result = new List<ResultPolicyDto>();

            foreach (var policy in policies)
            {
                result.Add(MapToResultDto(policy));
            }

            return result;
        }

        public List<ResultPolicyDto> GetPoliciesByCity(string city)
        {
            var policies = _policyRepository.GetPoliciesByCity(city);
            var result = new List<ResultPolicyDto>();

            foreach (var policy in policies)
            {
                result.Add(MapToResultDto(policy));
            }

            return result;
        }

        public List<ResultPolicyDto> GetPoliciesByDateRange(DateTime startDate, DateTime endDate)
        {
            var policies = _policyRepository.GetPoliciesByDateRange(startDate, endDate);
            var result = new List<ResultPolicyDto>();

            foreach (var policy in policies)
            {
                result.Add(MapToResultDto(policy));
            }

            return result;
        }

        public void Create(CreatePolicyDto createDto)
        {
            _createValidator.ValidateAndThrow(createDto);

            // Poliçe numarası otomatik oluştur
            if (string.IsNullOrEmpty(createDto.PolicyNumber))
                createDto.PolicyNumber = GeneratePolicyNumber();

            var policy = createDto.Adapt<Policy>();
            _policyRepository.Create(policy);
        }

        public void Update(UpdatePolicyDto updateDto)
        {
            _updateValidator.ValidateAndThrow(updateDto);

            var policy = _policyRepository.GetById(updateDto.Id);
            if (policy == null)
                throw new KeyNotFoundException("Poliçe bulunamadı");

            updateDto.Adapt(policy);
            _policyRepository.Update(policy);
        }

        public void Delete(int id)
        {
            var policy = _policyRepository.GetById(id);
            if (policy == null)
                throw new KeyNotFoundException("Poliçe bulunamadı");

            _policyRepository.Delete(policy);
        }

        public Dictionary<string, int> GetPolicyCountByCity()
        {
            var policies = _policyRepository.GetAll();
            return policies
                .GroupBy(p => p.User?.City ?? "Bilinmiyor")      // Customer yerine User
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<string, decimal> GetTotalPremiumByCity()
        {
            var policies = _policyRepository.GetAll();
            return policies
                .GroupBy(p => p.User?.City ?? "Bilinmiyor")      // Customer yerine User
                .ToDictionary(g => g.Key, g => g.Sum(p => p.PremiumAmount));
        }

        public string GeneratePolicyNumber()
        {
            var lastPolicy = _policyRepository.GetAll().OrderByDescending(p => p.Id).FirstOrDefault();
            int lastNumber = 1000;

            if (lastPolicy != null && !string.IsNullOrEmpty(lastPolicy.PolicyNumber))
            {
                var numberPart = lastPolicy.PolicyNumber.Replace("PLC-", "");
                if (int.TryParse(numberPart, out int parsed))
                    lastNumber = parsed + 1;
            }

            return $"PLC-{lastNumber}";
        }

        // Yardımcı metot - Policy'den ResultPolicyDto'ya dönüşüm
        private ResultPolicyDto MapToResultDto(Policy policy)
        {
            if (policy == null) return null;

            return new ResultPolicyDto
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                StartDate = policy.StartDate,
                EndDate = policy.EndDate,
                PremiumAmount = policy.PremiumAmount,
                CreatedAt = policy.CreatedAt,
                UserId = policy.UserId,
                UserFullName = policy.User != null
                    ? $"{policy.User.FirstName} {policy.User.LastName}"
                    : "Belirtilmemiş",
                UserEmail = policy.User?.Email,
                UserCity = policy.User?.City,
                InsurancePackageId = policy.InsurancePackageId,
                PackageName = policy.InsurancePackage?.PackageName,
                PackageBasePrice = policy.InsurancePackage?.BasePrice ?? 0
            };
        }
    }
}