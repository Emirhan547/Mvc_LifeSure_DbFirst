using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.PolicySaleDataServices;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class DataGeneratorController : AdminBaseController
    {
        private readonly IPolicySaleDataService _saleDataService;

        public DataGeneratorController(
            IAdminLogService logService,
            IPolicySaleDataService saleDataService) : base(logService)
        {
            _saleDataService = saleDataService;
        }

        // GET: Admin/DataGenerator
        public ActionResult Index()
        {
            var allSales = _saleDataService.GetAll();
            var currentYear = DateTime.Today.Year;
            var monthlyCounts = allSales
                .Where(x => x.SaleDate.Year == currentYear)
                .GroupBy(x => x.SaleDate.Month)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.SaleCount));

            var monthLabels = Enumerable.Range(1, 12)
                .Select(month => CultureInfo.GetCultureInfo("tr-TR").DateTimeFormat.GetAbbreviatedMonthName(month))
                .ToList();

            var monthValues = Enumerable.Range(1, 12)
                .Select(month => monthlyCounts.ContainsKey(month) ? monthlyCounts[month] : 0)
                .ToList();

            ViewBag.TotalPolicies = allSales.Sum(x => x.SaleCount);
            ViewBag.TotalPremium = allSales.Sum(x => x.TotalPremium);
            ViewBag.ChartYear = currentYear;
            ViewBag.MonthLabels = monthLabels;
            ViewBag.MonthValues = monthValues;
            return View();
        }

        // GET: Admin/DataGenerator/ViewData
        public ActionResult ViewData()
        {
            return View(_saleDataService.GetAll());
        }

        // POST: Admin/DataGenerator/GeneratePolicies  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GeneratePolicies(int count)
        {
            if (count < 100 || count > 5000)
                return Json(new { success = false, message = "Geçerli bir adet girin (100–5000)." });

            try
            {
                var generated = await _saleDataService.GeneratePoliciesFromGeminiAsync(count);
                LogAction($"{count} adet poliçe verisi oluşturuldu (gerçekleşen: {generated})", "Generate", "PolicySaleData");
                return Json(new
                {
                    success = true,
                    message = $"{count} adet talep edildi; {generated} kayıt başarıyla oluşturuldu.",
                    count = generated
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/DataGenerator/ClearAllData  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClearAllData()
        {
            try
            {
                _saleDataService.DeleteAll();
                LogAction("Tüm poliçe satış verileri silindi", "Clear", "PolicySaleData");
                return Json(new { success = true, message = "Tüm veriler başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
