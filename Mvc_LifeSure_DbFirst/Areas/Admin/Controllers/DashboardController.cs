using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.AppUserServices;
using Mvc_LifeSure_DbFirst.Services.InsurancePackageServices;
using Mvc_LifeSure_DbFirst.Services.PolicyServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        private readonly IAppUserService _userService;
        private readonly IInsurancePackageService _packageService;
        private readonly IPolicyService _policyService;

        public DashboardController(
            IAdminLogService logService,
            IAppUserService userService,
            IInsurancePackageService packageService,
            IPolicyService policyService) : base(logService)
        {
            _userService = userService;
            _packageService = packageService;
            _policyService = policyService;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.TotalUsers = await _userService.GetTotalUserCountAsync();
            ViewBag.TotalPackages = _packageService.GetAll().Count;
            ViewBag.TotalPolicies = _policyService.GetAll().Count;
            ViewBag.ActivePackages = _packageService.GetActivePackages().Count;
            ViewBag.TotalPremium = _policyService.GetAll().Sum(x => x.PremiumAmount);

            // Şehir bazlı poliçe dağılımı
            ViewBag.CityDistribution = _policyService.GetPolicyCountByCity();

            // Son 10 poliçe
            ViewBag.RecentPolicies = _policyService.GetAllSimple().OrderByDescending(x => x.Id).Take(10).ToList();

            // Son loglar
            var recentLogs = _logService.GetAll().OrderByDescending(x => x.Id).Take(10).ToList();
            ViewBag.RecentLogs = recentLogs;

            LogAction("Dashboard görüntülendi", "View", "Dashboard");

            return View();
        }
    }
}