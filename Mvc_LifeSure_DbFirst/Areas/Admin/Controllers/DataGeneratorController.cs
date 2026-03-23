using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.PolicySaleDataServices;
using System;
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
            ViewBag.TotalPolicies = _saleDataService.GetTotalPolicyCount();
            ViewBag.TotalPremium = _saleDataService.GetTotalPremium();
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
