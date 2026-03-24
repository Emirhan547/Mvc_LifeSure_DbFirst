using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.PolicyDtos;
using Mvc_LifeSure_DbFirst.Dtos.PolicyManagementDtos;
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
            return _policyRepository.GetAllWithDetails()
                 .Select(MapToResultDto)
                 .ToList();
        }

        public List<ResultPolicySimpleDto> GetAllSimple()
        {
            return _policyRepository.GetAllWithDetails()
                 .Select(policy => new ResultPolicySimpleDto
                 {
                     Id = policy.Id,
                     PolicyNumber = policy.PolicyNumber,
                     StartDate = policy.StartDate,
                     PremiumAmount = policy.PremiumAmount,
                     UserFullName = policy.User != null
                        ? $"{policy.User.FirstName} {policy.User.LastName}"
                        : "Belirtilmemiş",
                     PackageName = policy.InsurancePackage?.PackageName
                 })
                .ToList();
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
            return _policyRepository.GetPoliciesByUser(userId)
                 .Select(MapToResultDto)
                 .ToList();

           
        }
        public List<ResultPolicyDto> GetPoliciesByPackage(int packageId)
        {
            return _policyRepository.GetPoliciesByPackage(packageId)
                .Select(MapToResultDto)
                .ToList();
        }
        public List<ResultPolicyDto> GetPoliciesByCity(string city)
        {
            return _policyRepository.GetPoliciesByCity(city)
                .Select(MapToResultDto)
                .ToList();
        }
        public List<ResultPolicyDto> GetPoliciesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _policyRepository.GetPoliciesByDateRange(startDate, endDate)
                .Select(MapToResultDto)
                .ToList();
        }

        public List<ResultPolicyDto> GetFilteredPolicies(PolicyManagementFilterDto filter)
        {
            filter = filter ?? new PolicyManagementFilterDto();
            var today = DateTime.Today;

            IEnumerable<ResultPolicyDto> query = GetAll();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                query = query.Where(policy => ContainsIgnoreCase(policy.PolicyNumber, filter.SearchTerm)
                     || ContainsIgnoreCase(policy.UserFullName, filter.SearchTerm)
                     || ContainsIgnoreCase(policy.PackageName, filter.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                query = query.Where(policy => string.Equals(policy.UserCity, filter.City, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.PackageId.HasValue)
            {
                query = query.Where(policy => policy.InsurancePackageId == filter.PackageId.Value);
            }

            switch (filter.Status?.Trim().ToLowerInvariant())
            {
                case "active":
                    query = query.Where(policy => policy.EndDate >= today);
                    break;
                case "expired":
                    query = query.Where(policy => policy.EndDate < today);
                    break;
                case "expiring":
                    query = query.Where(policy => policy.EndDate >= today && policy.EndDate <= today.AddDays(30));
                    break;
            }

            return query.OrderByDescending(policy => policy.CreatedAt).ToList();
        }

        public PolicySummaryDto GetPolicySummary(List<ResultPolicyDto> policies, DateTime? referenceDate = null)
        {
            var policyList = policies ?? new List<ResultPolicyDto>();
            var today = referenceDate?.Date ?? DateTime.Today;
            var expiringLimit = today.AddDays(30);


            return new PolicySummaryDto
            {
                TotalPolicyCount = policyList.Count,
                ActivePolicyCount = policyList.Count(policy => policy.EndDate >= today),
                ExpiredPolicyCount = policyList.Count(policy => policy.EndDate < today),
                ExpiringSoonCount = policyList.Count(policy => policy.EndDate >= today && policy.EndDate <= expiringLimit),
                TotalPremium = policyList.Sum(policy => policy.PremiumAmount)
            };
        }

        public List<string> GetAvailableCities()
        {
            return GetAll()
                .Select(policy => policy.UserCity)
                .Where(city => !string.IsNullOrWhiteSpace(city))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(city => city)
                .ToList();
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
        private static bool ContainsIgnoreCase(string source, string value)
        {
            return !string.IsNullOrWhiteSpace(source)
                && source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
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
            return _policyRepository.GetAllWithDetails()
                 .GroupBy(p => p.User?.City ?? "Bilinmiyor")     // Customer yerine User
                 .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<string, decimal> GetTotalPremiumByCity()
        {
            return _policyRepository.GetAllWithDetails()
                 .GroupBy(p => p.User?.City ?? "Bilinmiyor")     // Customer yerine User
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