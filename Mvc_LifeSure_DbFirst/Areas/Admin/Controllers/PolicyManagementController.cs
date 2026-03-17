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

        public async Task<ActionResult> Index()
        {
            var policies = _policyService.GetAll();
            return View(policies);
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
            var policies = _policyService.GetPoliciesByCity(city);
            ViewBag.City = city;
            return View("Index", policies);
        }

        public ActionResult FilterByDate(DateTime startDate, DateTime endDate)
        {
            var policies = _policyService.GetPoliciesByDateRange(startDate, endDate);
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return View("Index", policies);
        }
    }
}