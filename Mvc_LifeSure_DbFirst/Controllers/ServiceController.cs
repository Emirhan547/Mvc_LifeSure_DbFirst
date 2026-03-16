using Mvc_LifeSure_DbFirst.Services.ServiceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServicesService _servicesService;

        public ServiceController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }
        public PartialViewResult Index()
        {
            var services = _servicesService.GetAll();
            return PartialView(services);
        }
    }
}