using Mvc_LifeSure_DbFirst.Services.FeatureServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class FeatureController : Controller
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }
        public PartialViewResult Index()
        {
            var features = _featureService.GetAll();
            return PartialView(features);
        }
    }
}