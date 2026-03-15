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

        public PartialViewResult Index()
        {
            return PartialView();
        }
    }
}