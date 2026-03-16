using Mvc_LifeSure_DbFirst.Services.AboutServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }
        public PartialViewResult Index()
        {
            var abouts = _aboutService.GetAll();
            return PartialView(abouts);
        }
    }
}