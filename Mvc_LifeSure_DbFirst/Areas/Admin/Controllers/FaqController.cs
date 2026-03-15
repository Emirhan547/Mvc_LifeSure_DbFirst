using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.FaqDtos;
using Mvc_LifeSure_DbFirst.Services.FaqServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class FaqController : Controller
    {
        private readonly IFaqService _faqService;

        public FaqController(IFaqService faqService)
        {
            _faqService = faqService;

        }

        public ActionResult Index()
        {
            var faqs=_faqService.GetAll();
            return View(faqs);
        }
        public ActionResult DeleteFaq(int id)
        {
            _faqService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult UpdateFaq(int id)
        {
            var faqs=_faqService.GetById(id);
            return View(faqs);
        }
        public ActionResult UpdateFaq(UpdateFaqDto updateFaqDto)
        {
            _faqService.Update(updateFaqDto);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult CreateFaq (CreateFaqDto createFaqDto)
        {
            _faqService.Create(createFaqDto);
            return RedirectToAction(nameof(Index));
        }
    }
}