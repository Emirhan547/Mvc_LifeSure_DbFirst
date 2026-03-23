using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.MLServices;
using Mvc_LifeSure_DbFirst.Services.PolicySaleDataServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class ForecastController : AdminBaseController
    {
        private readonly IForecastService _forecastService;
        private readonly IPolicySaleDataService _saleDataService;

        public ForecastController(
            IAdminLogService logService,
            IForecastService forecastService,
            IPolicySaleDataService saleDataService) : base(logService)
        {
            _forecastService = forecastService;
            _saleDataService = saleDataService;
        }

        // GET: Admin/Forecast
        public ActionResult Index()
        {
            var cities = GetDistinctCities();
            ViewBag.Cities = new SelectList(cities);
            return View();
        }

        // GET: Admin/Forecast/Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }

        // POST: Admin/Forecast/GetForecast  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetForecast(string city, int months = 3)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Json(new { success = false, message = "Şehir seçilmedi." });

            try
            {
                var forecast = await _forecastService.ForecastCitySalesAsync(city, months);
                LogAction($"{city} için {months} aylık tahmin yapıldı", "Forecast", "MLForecast");
                return Json(new { success = true, data = forecast });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Forecast/GetAllForecasts  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetAllForecasts(int months = 3)
        {
            try
            {
                var forecasts = await _forecastService.ForecastAllCitiesAsync(months);
                return Json(new { success = true, data = forecasts });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Forecast/GetYearlyForecast  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetYearlyForecast(int year)
        {
            try
            {
                var forecasts = await _forecastService.GetYearlyForecastAsync(year);
                return Json(new { success = true, data = forecasts });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Forecast/TrainModel  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TrainModel(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Json(new { success = false, message = "Şehir seçilmedi." });

            try
            {
                var result = await _forecastService.TrainModelForCityAsync(city);
                LogAction($"{city} için ML model eğitildi. R²: {result.RScore:F2}", "TrainModel", "MLForecast");
                return Json(new { success = result.Success, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Forecast/ValidateForecast  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ValidateForecast(string city, int months = 6)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Json(new { success = false, message = "Şehir seçilmedi." });

            try
            {
                var endDate = DateTime.Now.AddMonths(-1);
                var startDate = endDate.AddMonths(-months * 2);
                var accuracy = await _forecastService.ValidateForecastAsync(city, startDate, endDate);
                return Json(new { success = true, accuracy });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Forecast/ExportForecast
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExportForecast(string city, int months)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Json(new { success = false, message = "Şehir seçilmedi." });

            try
            {
                var data = await _forecastService.ExportForecastToExcelAsync(city, months);
                LogAction($"{city} için {months} aylık tahmin verisi dışa aktarıldı", "Export", "MLForecast");
                return File(data, "text/csv", $"tahmin_{city}_{DateTime.Now:yyyyMMdd}.csv");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/Forecast/GetDashboardData  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetDashboardData()
        {
            try
            {
                var allForecasts = await _forecastService.ForecastAllCitiesAsync(6);
                var yearlyForecast = await _forecastService.GetYearlyForecastAsync(DateTime.Now.Year + 1);
                return Json(new { success = true, forecasts = allForecasts, yearly = yearlyForecast });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ---- Özel yardımcı ----
        private System.Collections.Generic.List<string> GetDistinctCities()
        {
            return _saleDataService.GetAll()
                .Select(x => x.City)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }
    }
}