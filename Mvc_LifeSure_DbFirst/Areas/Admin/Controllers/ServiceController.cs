using Mvc_LifeSure_DbFirst.Dtos.ServiceDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.ServiceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class ServiceController : AdminBaseController
    {
        private readonly IServicesService _services;

        public ServiceController(
             IAdminLogService logService,
             IServicesService services) : base(logService)
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
        [ValidateAntiForgeryToken]
        public ActionResult CreateService(CreateServicesDto create)
        {
            _services.Create(create);
            LogAction("Hizmet kaydı oluşturuldu", "Create", "Services");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult UpdateService(int id)
        {
            var services = _services.GetById(id);
            return View(services);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateService(UpdateServicesDto update)
        {
            _services.Update(update);
            LogAction("Hizmet kaydı güncellendi", "Update", "Services", update.Id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteService(int id)
        {
            _services.Delete(id);
            LogAction("Hizmet kaydı silindi", "Delete", "Services", id);
            return RedirectToAction(nameof(Index));
        }
    }
}