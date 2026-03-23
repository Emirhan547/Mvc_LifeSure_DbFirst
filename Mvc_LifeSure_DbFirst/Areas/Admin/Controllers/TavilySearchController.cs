using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.AIServices;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class TavilySearchController : AdminBaseController
    {
        private readonly ITavilyService _tavilyService;

        public TavilySearchController(
            IAdminLogService logService,
            ITavilyService tavilyService) : base(logService)
        {
            _tavilyService = tavilyService;
        }

        // GET: Admin/TavilySearch
        public ActionResult Index() => View();

        // POST: Admin/TavilySearch/Search  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Search(string query, int maxResults = 5)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new { success = false, message = "Arama sorgusu boş olamaz." });

            try
            {
                var result = await _tavilyService.SearchAsync(query, maxResults);
                LogAction($"Tavily arama: '{query}'", "Search", "TavilySearch");
                return Json(new { success = true, data = result, query });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/TavilySearch/QuickAnswer  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> QuickAnswer(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Json(new { success = false, message = "Sorgu boş olamaz." });

            try
            {
                var answer = await _tavilyService.SearchAndGetAnswerAsync(query);
                LogAction($"Tavily hızlı cevap: '{query}'", "QuickAnswer", "TavilySearch");
                return Json(new { success = true, answer });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/TavilySearch/SearchInsuranceNews  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SearchInsuranceNews()
        {
            try
            {
                var query = $"sigorta sektörü güncel haberler {DateTime.Now.Year}";
                var result = await _tavilyService.SearchAsync(query, 8);
                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // Admin/TavilySearchController'a ekleyin
        public ActionResult TestLog()
        {
            try
            {
                string testPath = Server.MapPath("~/App_Data/test_controller.txt");
                System.IO.File.WriteAllText(testPath, $"Test: {DateTime.Now}");
                return Content($"Log yazıldı: {testPath}");
            }
            catch (Exception ex)
            {
                return Content($"Hata: {ex.Message}");
            }
        }
    }
}