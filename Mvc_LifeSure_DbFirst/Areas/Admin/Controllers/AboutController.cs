using Mvc_LifeSure_DbFirst.Dtos.AboutDtos;
using Mvc_LifeSure_DbFirst.Services.AboutServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class AboutController : Controller
    {
       private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
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
        public ActionResult CreateAbout(CreateAboutDto create)
        {
           _aboutService.Create(create);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateAbout(int id)
        {
            var abouts= _aboutService.GetById(id);
            return View(abouts);
        }
        [HttpPost]
        public ActionResult UpdateAbout(UpdateAboutDto update)
        {
            _aboutService.Update(update);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteAbout(int id)
        {
            _aboutService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}