using Mvc_LifeSure_DbFirst.Dtos.PolicySaleDataDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.PolicySaleDataServices
{
    public interface IPolicySaleDataService
    {
        List<ResultPolicySaleDataDto> GetAll();
        ResultPolicySaleDataDto GetById(int id);
        void Create(CreatePolicySaleDataDto createDto);
        void CreateBulk(List<CreatePolicySaleDataDto> createDtos);
        void Delete(int id);
        void DeleteAll();
        List<ResultPolicySaleDataDto> GetSalesByCity(string city);
        List<ResultPolicySaleDataDto> GetSalesByYear(int year);
        Dictionary<string, int> GetMonthlySalesByCity(string city, int year);
        int GetTotalPolicyCount();
        decimal GetTotalPremium();
        Task<int> GeneratePoliciesFromGeminiAsync(int count);
    }
}
