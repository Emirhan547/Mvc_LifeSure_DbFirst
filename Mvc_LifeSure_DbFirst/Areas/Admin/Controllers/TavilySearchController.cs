using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.AIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Search(string query, int maxResults = 5)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return Json(new { success = false, message = "Arama sorgusu boş olamaz." });
                }

                var result = await _tavilyService.SearchAsync(query, maxResults);

                // Loglama
                LogAction(
                    $"Tavily AI ile arama yapıldı: '{query}'",
                    "Search",
                    "TavilySearch"
                );

                return Json(new
                {
                    success = true,
                    data = result,
                    query = query
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> QuickAnswer(string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return Json(new { success = false, message = "Sorgu boş olamaz." });
                }

                var answer = await _tavilyService.SearchAndGetAnswerAsync(query);

                // Loglama
                LogAction(
                    $"Tavily AI ile hızlı cevap alındı: '{query}'",
                    "QuickAnswer",
                    "TavilySearch"
                );

                return Json(new { success = true, answer = answer });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SearchNews(string topic, int days = 7)
        {
            try
            {
                var query = $"son {days} gün {topic} haberleri";
                var result = await _tavilyService.SearchAsync(query, 10);

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SearchInsuranceNews()
        {
            try
            {
                var query = "sigorta sektörü güncel haberler 2025";
                var result = await _tavilyService.SearchAsync(query, 8);

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}