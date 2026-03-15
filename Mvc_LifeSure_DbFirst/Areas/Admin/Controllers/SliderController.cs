using Mvc_LifeSure_DbFirst.Dtos.SliderDtos;
using Mvc_LifeSure_DbFirst.Services.SliderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public ActionResult Index()
        {
            var sliders = _sliderService.GetAll();
            return View(sliders);
        }
        [HttpGet]
        public ActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateSlider(CreateSliderDto create)
        {
            _sliderService.Create(create);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateSlider(int id)
        {
            var sliders = _sliderService.GetById(id);
            return View(sliders);
        }
        [HttpPost]
        public ActionResult UpdateSlider(UpdateSliderDto update)
        {
            _sliderService.Update(update);
            return RedirectToAction(nameof(Index));
        }
       
        public ActionResult DeleteSlider(int id)
        {
            _sliderService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}