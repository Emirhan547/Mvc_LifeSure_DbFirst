using Mvc_LifeSure_DbFirst.Dtos.FeatureDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.FeatureServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class FeatureController : AdminBaseController
    {
        private readonly IFeatureService _featureService;

        public FeatureController(
             IAdminLogService logService,
             IFeatureService featureService) : base(logService)
        {
            _featureService = featureService;
        }

        public ActionResult Index()
        {
            var features = _featureService.GetAll();
            return View(features);
        }
        [HttpGet]
        public ActionResult CreateFeature()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFeature(CreateFeatureDto createFeatureDto)
        {
            _featureService.Create(createFeatureDto);
            LogAction("Özellik kaydı oluşturuldu", "Create", "Features");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateFeature(int id)
        {
            var features = _featureService.GetById(id);
                return View(features);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFeature (UpdateFeatureDto updateFeatureDto)
        {
            _featureService.Update(updateFeatureDto);
            LogAction("Özellik kaydı güncellendi", "Update", "Features", updateFeatureDto.Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFeature(int id)
        {
            _featureService.Delete(id);
            LogAction("Özellik kaydı silindi", "Delete", "Features", id);
            return RedirectToAction(nameof(Index));
        }
    }
}