using Mvc_LifeSure_DbFirst.Services.TeamServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        public PartialViewResult Index()
        {
            var teams = _teamService.GetAll();
            return PartialView(teams);
        }
    }
}