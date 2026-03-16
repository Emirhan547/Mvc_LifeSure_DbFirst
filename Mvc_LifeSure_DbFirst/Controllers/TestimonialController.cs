using Mvc_LifeSure_DbFirst.Services.TestimonialServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ITestimonialService testimonialService)
        {
            _testimonialService = testimonialService;
        }
        public PartialViewResult Index()
        {
            var testimonials = _testimonialService.GetAll();
            return PartialView(testimonials);
        }
    }
}