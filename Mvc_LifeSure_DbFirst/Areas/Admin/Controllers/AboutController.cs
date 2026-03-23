using Mvc_LifeSure_DbFirst.Dtos.AboutDtos;
using Mvc_LifeSure_DbFirst.Services.AboutServices;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class AboutController : AdminBaseController
    {
       private readonly IAboutService _aboutService;

        public AboutController(
              IAdminLogService logService,
              IAboutService aboutService) : base(logService)
        {
            _aboutService = aboutService;
        }

        public ActionResult Index()
        {
            var abouts = _aboutService.GetAll();
            return View(abouts);
        }
        public ActionResult CreateAbout()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAbout(CreateAboutDto create)
        {
            _aboutService.Create(create);
            LogAction("About kaydı oluşturuldu", "Create", "Abouts");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet] 
        public ActionResult UpdateAbout(int id)
        {
            var abouts= _aboutService.GetById(id);
            return View(abouts);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAbout(UpdateAboutDto update)
        {
            _aboutService.Update(update);
            LogAction("About kaydı güncellendi", "Update", "Abouts", update.Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAbout(int id)
        {
            _aboutService.Delete(id);
            LogAction("About kaydı silindi", "Delete", "Abouts", id);
            return RedirectToAction("Index");
        }
    }
}