using Mvc_LifeSure_DbFirst.Services.SliderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }
        public PartialViewResult Index()
        {
            var sliders = _sliderService.GetAll();
            return PartialView(sliders);
        }
    }
}