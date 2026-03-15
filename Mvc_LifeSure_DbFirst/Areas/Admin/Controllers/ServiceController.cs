using Mvc_LifeSure_DbFirst.Dtos.ServiceDtos;
using Mvc_LifeSure_DbFirst.Services.ServiceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServicesService _services;

        public ServiceController(IServicesService services)
        {
            _services = services;
        }

        public ActionResult Index()
        {
            var services=_services.GetAll();
            return View(services);
        }
        [HttpGet]
        public ActionResult CreateService()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateService(CreateServicesDto create)
        {
            _services.Create(create);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult UpdateService(int id)
        {
            var services = _services.GetById(id);
            return View(services);
        }
        [HttpPost]
        public ActionResult UpdateService(UpdateServicesDto update)
        {
            _services.Update(update);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteService(int id)
        {
            _services.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}