using Mvc_LifeSure_DbFirst.Dtos.FeatureDtos;
using Mvc_LifeSure_DbFirst.Services.FeatureServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
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
        public ActionResult CreateFeature(CreateFeatureDto createFeatureDto)
        {
            _featureService.Create(createFeatureDto);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateFeature(int id)
        {
            var features = _featureService.GetById(id);
                return View(features);
        }
        [HttpPost]
        public ActionResult UpdateFeature (UpdateFeatureDto updateFeatureDto)
        {
            _featureService.Update(updateFeatureDto);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult DeleteFeature(int id)
        {
            _featureService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}