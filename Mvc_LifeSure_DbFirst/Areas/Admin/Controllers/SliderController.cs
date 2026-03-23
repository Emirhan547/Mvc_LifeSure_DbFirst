using Mvc_LifeSure_DbFirst.Dtos.SliderDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.SliderServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class SliderController : AdminBaseController
    {
        private readonly ISliderService _sliderService;

        public SliderController(
             IAdminLogService logService,
             ISliderService sliderService) : base(logService)
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
        [ValidateAntiForgeryToken]
        public ActionResult CreateSlider(CreateSliderDto create)
        {
            _sliderService.Create(create);
            LogAction("Slider kaydı oluşturuldu", "Create", "Sliders");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateSlider(int id)
        {
            var sliders = _sliderService.GetById(id);
            return View(sliders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSlider(UpdateSliderDto update)
        {
            _sliderService.Update(update);
            LogAction("Slider kaydı güncellendi", "Update", "Sliders", update.Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSlider(int id)
        {
            _sliderService.Delete(id);
            LogAction("Slider kaydı silindi", "Delete", "Sliders", id);
            return RedirectToAction(nameof(Index));
        }
    }
}