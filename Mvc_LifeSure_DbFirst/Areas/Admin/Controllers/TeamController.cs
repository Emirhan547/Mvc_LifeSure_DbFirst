using Mvc_LifeSure_DbFirst.Dtos.TeamDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.TeamServices;
using System;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class TeamController : AdminBaseController
    {
        private readonly ITeamService _teamService;

        public TeamController(
            IAdminLogService logService,
            ITeamService teamService) : base(logService)
        {
            _teamService = teamService;
        }

        public ActionResult Index()
        {
            return View(_teamService.GetAll());
        }

        [HttpGet]
        public ActionResult CreateTeam() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeam(CreateTeamDto create)
        {
            _teamService.Create(create);
            LogAction("Ekip kaydı oluşturuldu", "Create", "Teams");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult UpdateTeam(int id)
        {
            return View(_teamService.GetById(id));   // BUG FIX: artık UpdateTeamDto döner
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTeam(UpdateTeamDto update)
        {
            _teamService.Update(update);
            LogAction("Ekip kaydı güncellendi", "Update", "Teams", update.Id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTeam(int id)
        {
            _teamService.Delete(id);
            LogAction("Ekip kaydı silindi", "Delete", "Teams", id);
            return RedirectToAction(nameof(Index));
        }
    }
}