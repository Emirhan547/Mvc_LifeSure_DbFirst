using Mvc_LifeSure_DbFirst.Dtos.TestimonialDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.TestimonialServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class TestimonialController : AdminBaseController
    {
       private readonly ITestimonialService _testimonialService;

        public TestimonialController(
             IAdminLogService logService,
             ITestimonialService testimonialService) : base(logService)
        {
            _testimonialService = testimonialService;
        }

        public ActionResult Index()
        {
            var testimonials=_testimonialService.GetAll();
            return View(testimonials);
        }

        [HttpGet]
        public ActionResult CreateTestimonial()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTestimonial(CreateTestimonialDto create)
        {
            _testimonialService.Create(create);
            LogAction("Referans kaydı oluşturuldu", "Create", "Testimonials");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateTestimonial(int id)
        {
            var testimonials=_testimonialService.GetById(id);
            return View(testimonials);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTestimonial(UpdateTestimonialDto update)
        {
            _testimonialService.Update(update);
            LogAction("Referans kaydı güncellendi", "Update", "Testimonials", update.Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTestimonial(int id)
        {
            _testimonialService.Delete(id);
            LogAction("Referans kaydı silindi", "Delete", "Testimonials", id);
            return RedirectToAction(nameof(Index));
        }
    }
}