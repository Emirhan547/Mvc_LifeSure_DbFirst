using Mvc_LifeSure_DbFirst.Dtos.PolicyDtos;
using Mvc_LifeSure_DbFirst.Dtos.PolicyManagementDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.PolicyServices
{
    public interface IPolicyService
    {
        List<ResultPolicyDto> GetAll();
        List<ResultPolicySimpleDto> GetAllSimple();
        ResultPolicyDto GetById(int id);
        ResultPolicyDto GetPolicyWithDetails(int id);
        List<ResultPolicyDto> GetPoliciesByUser(string userId);           // string oldu
        List<ResultPolicyDto> GetPoliciesByPackage(int packageId);
        List<ResultPolicyDto> GetPoliciesByCity(string city);
        List<ResultPolicyDto> GetPoliciesByDateRange(DateTime startDate, DateTime endDate);
        void Create(CreatePolicyDto createDto);
        void Update(UpdatePolicyDto updateDto);
        void Delete(int id);
        Dictionary<string, int> GetPolicyCountByCity();
        Dictionary<string, decimal> GetTotalPremiumByCity();
        string GeneratePolicyNumber();
        List<ResultPolicyDto> GetFilteredPolicies(PolicyManagementFilterDto filter);
        PolicySummaryDto GetPolicySummary(List<ResultPolicyDto> policies, DateTime? referenceDate = null);
        List<string> GetAvailableCities();
    }
}