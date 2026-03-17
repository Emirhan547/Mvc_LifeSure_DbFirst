using Mvc_LifeSure_DbFirst.Dtos.InsurancePackageDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.InsurancePackageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class PackageManagementController : AdminBaseController
    {
        private readonly IInsurancePackageService _packageService;

        public PackageManagementController(
            IAdminLogService logService,
            IInsurancePackageService packageService) : base(logService)
        {
            _packageService = packageService;
        }

        public ActionResult Index()
        {
            var packages = _packageService.GetAll();
            return View(packages);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateInsurancePackageDto createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _packageService.Create(createDto);

                    LogAction(
                        $"{createDto.PackageName} paketi oluşturuldu",
                        "Create",
                        "InsurancePackages"
                    );

                    TempData["SuccessMessage"] = "Paket başarıyla oluşturuldu.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(createDto);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var package = _packageService.GetById(id);
                var updateDto = new UpdateInsurancePackageDto
                {
                    Id = package.Id,
                    PackageName = package.PackageName,
                    Description = package.Description,
                    BasePrice = package.BasePrice,
                    CoveragePeriodMonths = package.CoveragePeriodMonths,
                    IsActive = package.IsActive
                };

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
        public ActionResult Edit(UpdateInsurancePackageDto updateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _packageService.Update(updateDto);

                    LogAction(
                        $"{updateDto.PackageName} paketi güncellendi",
                        "Update",
                        "InsurancePackages",
                        updateDto.Id
                    );

                    TempData["SuccessMessage"] = "Paket başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(updateDto);
        }

        public ActionResult Details(int id)
        {
            try
            {
                var package = _packageService.GetPackageWithPolicies(id);
                return View(package);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ToggleStatus(int id)
        {
            try
            {
                var package = _packageService.GetById(id);
                _packageService.TogglePackageStatus(id);

                LogAction(
                    $"{package.PackageName} paketi {(package.IsActive ? "pasifleştirildi" : "aktifleştirildi")}",
                    "Update",
                    "InsurancePackages",
                    id
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var package = _packageService.GetById(id);
                _packageService.Delete(id);

                LogAction(
                    $"{package.PackageName} paketi silindi",
                    "Delete",
                    "InsurancePackages",
                    id
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}