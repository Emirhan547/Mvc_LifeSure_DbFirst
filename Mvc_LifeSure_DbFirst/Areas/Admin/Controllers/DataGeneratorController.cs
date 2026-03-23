using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.PolicySaleDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        public ActionResult Index()
        {
            ViewBag.TotalPolicies = _saleDataService.GetTotalPolicyCount();
            ViewBag.TotalPremium = _saleDataService.GetTotalPremium();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GeneratePolicies(int count)
        {
            try
            {
                var generatedCount = await _saleDataService.GeneratePoliciesFromGeminiAsync(count);

                LogAction(
                    $"{count} adet poliçe verisi oluşturuldu (Gerçekleşen: {generatedCount} kayıt)",
                    "Generate",
                    "PolicySaleData"
                );

                return Json(new
                {
                    success = true,
                    message = $"{count} adet poliçe verisi başarıyla oluşturuldu.",
                    count = generatedCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ClearAllData()
        {
            try
            {
                _saleDataService.DeleteAll();

                LogAction(
                    "Tüm poliçe satış verileri silindi",
                    "Clear",
                    "PolicySaleData"
                );

                return Json(new { success = true, message = "Tüm veriler başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult ViewData()
        {
            var data = _saleDataService.GetAll();
            return View(data);
        }
    }
}