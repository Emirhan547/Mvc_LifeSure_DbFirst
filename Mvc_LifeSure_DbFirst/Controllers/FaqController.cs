using Mvc_LifeSure_DbFirst.Services.FaqServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class FaqController : Controller
    {
        private readonly IFaqService _faqService;

        public FaqController(IFaqService faqService)
        {
            _faqService = faqService;
        }
        public PartialViewResult Index()
        {
            var faqs = _faqService.GetAll();
            return PartialView(faqs);
        }
    }
}