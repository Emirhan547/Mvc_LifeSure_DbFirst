using Mvc_LifeSure_DbFirst.Dtos.PolicyDtos;
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
    public class PolicyManagementController : AdminBaseController
    {
        private readonly IPolicyService _policyService;
        private readonly IAppUserService _userService;
        private readonly IInsurancePackageService _packageService;

        public PolicyManagementController(
            IAdminLogService logService,
            IPolicyService policyService,
            IAppUserService userService,
            IInsurancePackageService packageService) : base(logService)
        {
            _policyService = policyService;
            _userService = userService;
            _packageService = packageService;
        }

        public ActionResult Index(string searchTerm, string city, string status, int? packageId)
        {
            var allPolicies = _policyService.GetAll();
            var policies = allPolicies;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                policies = policies.Where(x =>
                    x.PolicyNumber.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.UserFullName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.PackageName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                policies = policies.Where(x => string.Equals(x.UserCity, city, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (packageId.HasValue)
            {
                policies = policies.Where(x => x.InsurancePackageId == packageId.Value).ToList();
            }

            var today = DateTime.Today;
            if (string.Equals(status, "active", StringComparison.OrdinalIgnoreCase))
            {
                policies = policies.Where(x => x.EndDate >= today).ToList();
            }
            else if (string.Equals(status, "expired", StringComparison.OrdinalIgnoreCase))
            {
                policies = policies.Where(x => x.EndDate < today).ToList();
            }
            else if (string.Equals(status, "expiring", StringComparison.OrdinalIgnoreCase))
            {
                policies = policies.Where(x => x.EndDate >= today && x.EndDate <= today.AddDays(30)).ToList();
            }

            ViewBag.TotalPolicyCount = policies.Count;
            ViewBag.ActivePolicyCount = policies.Count(x => x.EndDate >= today);
            ViewBag.ExpiredPolicyCount = policies.Count(x => x.EndDate < today);
            ViewBag.ExpiringSoonCount = policies.Count(x => x.EndDate >= today && x.EndDate <= today.AddDays(30));
            ViewBag.TotalPremium = policies.Sum(x => x.PremiumAmount);
            ViewBag.Cities = allPolicies.Select(x => x.UserCity).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList();
            ViewBag.Packages = _packageService.GetActivePackages();
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedCity = city;
            ViewBag.SelectedStatus = status;
            ViewBag.SelectedPackageId = packageId;

            return View(policies.OrderByDescending(x => x.CreatedAt).ToList());
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Packages = _packageService.GetActivePackages();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreatePolicyDto createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    createDto.PolicyNumber = _policyService.GeneratePolicyNumber();
                    _policyService.Create(createDto);

                    var user = await _userService.GetUserByIdAsync(createDto.UserId);
                    var package = _packageService.GetById(createDto.InsurancePackageId);

                    LogAction(
                        $"{user?.FirstName} {user?.LastName} için {package?.PackageName} poliçesi oluşturuldu (No: {createDto.PolicyNumber})",
                        "Create",
                        "Policies"
                    );

                    TempData["SuccessMessage"] = "Poliçe başarıyla oluşturuldu.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Packages = _packageService.GetActivePackages();
            return View(createDto);
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var policy = _policyService.GetById(id);
                var updateDto = new UpdatePolicyDto
                {
                    Id = policy.Id,
                    PolicyNumber = policy.PolicyNumber,
                    StartDate = policy.StartDate,
                    EndDate = policy.EndDate,
                    PremiumAmount = policy.PremiumAmount,
                    UserId = policy.UserId,
                    InsurancePackageId = policy.InsurancePackageId
                };

                ViewBag.Users = await _userService.GetAllUsersAsync();
                ViewBag.Packages = _packageService.GetActivePackages();

                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdatePolicyDto updateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _policyService.Update(updateDto);

                    LogAction(
                        $"{updateDto.PolicyNumber} poliçesi güncellendi",
                        "Update",
                        "Policies",
                        updateDto.Id
                    );

                    TempData["SuccessMessage"] = "Poliçe başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewBag.Users = await _userService.GetAllUsersAsync();
            ViewBag.Packages = _packageService.GetActivePackages();
            return View(updateDto);
        }

        public ActionResult Details(int id)
        {
            try
            {
                var policy = _policyService.GetPolicyWithDetails(id);
                return View(policy);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var policy = _policyService.GetById(id);
                _policyService.Delete(id);

                LogAction(
                    $"{policy.PolicyNumber} poliçesi silindi",
                    "Delete",
                    "Policies",
                    id
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<ActionResult> UserPolicies(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return HttpNotFound();

            var policies = _policyService.GetPoliciesByUser(userId);
            ViewBag.User = user;

            return View(policies);
        }

        public ActionResult FilterByCity(string city)
        {
            return RedirectToAction(nameof(Index), new { city });
        }

        public ActionResult FilterByDate(DateTime startDate, DateTime endDate)
        {
            return RedirectToAction(nameof(Index), new { startDate, endDate });
        }
    }
}