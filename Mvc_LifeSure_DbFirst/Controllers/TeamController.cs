using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        public PartialViewResult Index()
        {
            return PartialView();
        }
    }
}