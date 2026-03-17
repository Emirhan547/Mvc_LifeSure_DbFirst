using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.ContactMessageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class ContactMessageController : AdminBaseController
    {
        private readonly IContactMessageService _contactService;

        public ContactMessageController(
            IAdminLogService logService,
            IContactMessageService contactService) : base(logService)
        {
            _contactService = contactService;
        }

        public ActionResult Index()
        {
            var messages = _contactService.GetAll();
            ViewBag.Stats = _contactService.GetMessageStatsByCategory();
            return View(messages);
        }

        public ActionResult Unreplied()
        {
            var messages = _contactService.GetUnrepliedMessages();
            return View("Index", messages);
        }

        public ActionResult Details(int id)
        {
            try
            {
                var message = _contactService.GetById(id);
                return View(message);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Reprocess(int id)
        {
            try
            {
                // ContactMessage service'e ReprocessMessageAsync eklenmeli
                // await _contactService.ReprocessMessageAsync(id);

                LogAction(
                    $"Mesaj yeniden işleniyor (ID: {id})",
                    "Reprocess",
                    "ContactMessages",
                    id
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _contactService.Delete(id);

                LogAction(
                    $"Mesaj silindi (ID: {id})",
                    "Delete",
                    "ContactMessages",
                    id
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}