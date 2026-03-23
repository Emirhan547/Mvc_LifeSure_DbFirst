using FluentValidation;
using Mvc_LifeSure_DbFirst.Dtos.FaqDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.FaqServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class FaqController : AdminBaseController
    {
        private readonly IFaqService _faqService;

        public FaqController(
             IAdminLogService logService,
             IFaqService faqService) : base(logService)
        {
            _faqService = faqService;

        }

        public ActionResult Index()
        {
            var faqs=_faqService.GetAll();
            return View(faqs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFaq(int id)
        {
            _faqService.Delete(id);
            LogAction("SSS kaydı silindi", "Delete", "Faqs", id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateFaq(int id)
        {
            var faqs=_faqService.GetById(id);
            return View(faqs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFaq(UpdateFaqDto updateFaqDto)
        {
            _faqService.Update(updateFaqDto);
            LogAction("SSS kaydı güncellendi", "Update", "Faqs", updateFaqDto.Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        
        public ActionResult CreateFaq()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFaq (CreateFaqDto createFaqDto)
        {
            _faqService.Create(createFaqDto);
            LogAction("SSS kaydı oluşturuldu", "Create", "Faqs");
            return RedirectToAction(nameof(Index));
        }
    }
}