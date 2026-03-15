using Mvc_LifeSure_DbFirst.Dtos.TeamDtos;
using Mvc_LifeSure_DbFirst.Services.TeamServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public ActionResult Index()
        {
            var testimonials = _teamService.GetAll();
            return View(testimonials);
        }
        [HttpGet]
        public ActionResult CreateTeam()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTeam(CreateTeamDto create)
        {
            _teamService.Create(create);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateTeam (int id)
        {
            var teams = _teamService.GetById(id);
            return View(teams);
        }
        [HttpPost]
        public ActionResult UpdateTeam(UpdateTeamDto update)
        {
            _teamService.Update(update);
            return RedirectToAction(nameof(Index));
        }
        public ActionResult DeleteTeam(int id)
        {
            _teamService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}