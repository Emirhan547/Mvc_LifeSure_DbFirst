using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.PolicySaleDataDtos;
using Mvc_LifeSure_DbFirst.Repositories.PolicySaleDataRepositories;
using Mvc_LifeSure_DbFirst.Services.AIServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.PolicySaleDataServices
{
    public class PolicySaleDataService : IPolicySaleDataService
    {
        private readonly IPolicySaleDataRepository _repository;
        private readonly IGeminiService _geminiService;

        public PolicySaleDataService(IPolicySaleDataRepository repository, IGeminiService geminiService)
        {
            _repository = repository;
            _geminiService = geminiService;
        }

        public List<ResultPolicySaleDataDto> GetAll()
        {
            var data = _repository.GetAll();
            return data.Adapt<List<ResultPolicySaleDataDto>>();
        }

        public ResultPolicySaleDataDto GetById(int id)
        {
            var data = _repository.GetById(id);
            if (data == null)
                throw new KeyNotFoundException("Veri bulunamadı");

            return data.Adapt<ResultPolicySaleDataDto>();
        }

        public void Create(CreatePolicySaleDataDto createDto)
        {
            var data = createDto.Adapt<PolicySaleData>();
            _repository.Create(data);
        }

        public void CreateBulk(List<CreatePolicySaleDataDto> createDtos)
        {
            foreach (var dto in createDtos)
            {
                var data = dto.Adapt<PolicySaleData>();
                _repository.Create(data);
            }
        }

        public void Delete(int id)
        {
            var data = _repository.GetById(id);
            if (data == null)
                throw new KeyNotFoundException("Veri bulunamadı");

            _repository.Delete(data);
        }

        public void DeleteAll()
        {
            var allData = _repository.GetAll();
            foreach (var data in allData)
            {
                _repository.Delete(data);
            }
        }

        public List<ResultPolicySaleDataDto> GetSalesByCity(string city)
        {
            var data = _repository.GetSalesByCity(city);
            return data.Adapt<List<ResultPolicySaleDataDto>>();
        }

        public List<ResultPolicySaleDataDto> GetSalesByYear(int year)
        {
            var data = _repository.GetSalesByYear(year);
            return data.Adapt<List<ResultPolicySaleDataDto>>();
        }

        public Dictionary<string, int> GetMonthlySalesByCity(string city, int year)
        {
            var data = _repository.GetSalesByCity(city)
                .Where(x => x.SaleDate.Year == year)
                .GroupBy(x => x.SaleDate.Month)
                .ToDictionary(
                    g => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    g => g.Sum(x => x.SaleCount)
                );

            return data;
        }

        public int GetTotalPolicyCount()
        {
            return _repository.GetAll().Sum(x => x.SaleCount);
        }

        public decimal GetTotalPremium()
        {
            return _repository.GetAll().Sum(x => x.TotalPremium);
        }

        public async Task<int> GeneratePoliciesFromGeminiAsync(int count)
        {
            try
            {
                var generatedPolicies = await _geminiService.GeneratePoliciesAsync(count);

                var groupedData = new List<CreatePolicySaleDataDto>();

                // Switch expression yerine if-else kullan
                foreach (var policy in generatedPolicies)
                {
                    int monthNumber = 1;
                    if (policy.Month == "Ocak") monthNumber = 1;
                    else if (policy.Month == "Şubat") monthNumber = 2;
                    else if (policy.Month == "Mart") monthNumber = 3;
                    else if (policy.Month == "Nisan") monthNumber = 4;
                    else if (policy.Month == "Mayıs") monthNumber = 5;
                    else if (policy.Month == "Haziran") monthNumber = 6;
                    else if (policy.Month == "Temmuz") monthNumber = 7;
                    else if (policy.Month == "Ağustos") monthNumber = 8;
                    else if (policy.Month == "Eylül") monthNumber = 9;
                    else if (policy.Month == "Ekim") monthNumber = 10;
                    else if (policy.Month == "Kasım") monthNumber = 11;
                    else if (policy.Month == "Aralık") monthNumber = 12;
                }

                // Gruplama işlemi - LINQ ile
                var grouped = generatedPolicies
                    .GroupBy(p => new { p.City, p.Year, p.Month })
                    .Select(g => new CreatePolicySaleDataDto
                    {
                        City = g.Key.City,
                        SaleDate = new DateTime(g.Key.Year, GetMonthNumber(g.Key.Month), 1),
                        SaleCount = g.Count(),
                        TotalPremium = g.Sum(p => p.PremiumAmount)
                    })
                    .ToList();

                CreateBulk(grouped);

                return grouped.Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Veri oluşturulurken hata: " + ex.Message);
            }
        }

        private int GetMonthNumber(string monthName)
        {
            if (monthName == "Ocak") return 1;
            if (monthName == "Şubat") return 2;
            if (monthName == "Mart") return 3;
            if (monthName == "Nisan") return 4;
            if (monthName == "Mayıs") return 5;
            if (monthName == "Haziran") return 6;
            if (monthName == "Temmuz") return 7;
            if (monthName == "Ağustos") return 8;
            if (monthName == "Eylül") return 9;
            if (monthName == "Ekim") return 10;
            if (monthName == "Kasım") return 11;
            if (monthName == "Aralık") return 12;
            return 1;
        }
    }
}