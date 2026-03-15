using Mvc_LifeSure_DbFirst.Dtos.TestimonialDtos;
using Mvc_LifeSure_DbFirst.Services.TestimonialServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class TestimonialController : Controller
    {
       private readonly ITestimonialService _testimonialService;

        public TestimonialController(ITestimonialService testimonialService)
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
        public ActionResult CreateTestimonial(CreateTestimonialDto create)
        {
            _testimonialService.Create(create);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateTestimonial(int id)
        {
            var testimonials=_testimonialService.GetById(id);
            return View(testimonials);
        }
        [HttpPost]
        public ActionResult UpdateTestimonial(UpdateTestimonialDto update)
        {
            _testimonialService.Update(update);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult DeleteTestimonial(int id)
        {
            _testimonialService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}