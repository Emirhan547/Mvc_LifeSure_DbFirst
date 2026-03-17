using Mvc_LifeSure_DbFirst.Dtos.MLDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.MLServices
{
    public interface IForecastService
    {
        Task<List<CityForecastResult>> ForecastCitySalesAsync(string city, int forecastMonths = 3);

        // Tüm şehirler için tahmin
        Task<Dictionary<string, List<CityForecastResult>>> ForecastAllCitiesAsync(int forecastMonths = 3);

        // Yıllık çeyreklik tahmin
        Task<List<YearlyForecastSummary>> GetYearlyForecastAsync(int year);

        // Model eğitimi
        Task<ModelTrainingResult> TrainModelForCityAsync(string city);

        // Tahmin doğruluğunu kontrol et
        Task<float> ValidateForecastAsync(string city, DateTime startDate, DateTime endDate);

        // Excel/CSV export için tahmin verileri
        Task<byte[]> ExportForecastToExcelAsync(string city, int months);
    }
}
