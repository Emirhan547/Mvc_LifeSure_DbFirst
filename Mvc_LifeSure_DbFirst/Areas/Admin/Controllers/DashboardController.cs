using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.AppUserServices;
using Mvc_LifeSure_DbFirst.Services.InsurancePackageServices;
using Mvc_LifeSure_DbFirst.Services.PolicyServices;
using Mvc_LifeSure_DbFirst.Dtos.DashboardDtos;
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
           
            var policies = _policyService.GetAll().OrderByDescending(x => x.CreatedAt).ToList();
            var activePackages = _packageService.GetActivePackages();
            var recentLogs = _logService.GetAll().OrderByDescending(x => x.Timestamp).Take(8).ToList();
            var now = DateTime.Today;

            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _userService.GetTotalUserCountAsync(),
                TotalPackages = _packageService.GetAll().Count,
                ActivePackages = activePackages.Count,
                TotalPolicies = policies.Count,
                ActivePolicies = policies.Count(x => x.EndDate >= now),
                ExpiringPolicies = policies.Count(x => x.EndDate >= now && x.EndDate <= now.AddDays(30)),
                TotalPremium = policies.Sum(x => x.PremiumAmount),
                AveragePremium = policies.Any() ? policies.Average(x => x.PremiumAmount) : 0,
                CityDistribution = _policyService.GetPolicyCountByCity(),
                PremiumDistribution = _policyService.GetTotalPremiumByCity(),
                RecentPolicies = _policyService.GetAllSimple().OrderByDescending(x => x.Id).Take(8).ToList(),
                RecentLogs = recentLogs,
                ExpiringPolicyList = policies.Where(x => x.EndDate >= now && x.EndDate <= now.AddDays(30)).Take(10).ToList()
            };

            LogAction("Dashboard görüntülendi", "View", "Dashboard");

           
            return View(model);
        }
    }
}