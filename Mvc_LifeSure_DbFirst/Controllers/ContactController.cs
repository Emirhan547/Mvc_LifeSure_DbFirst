using Mvc_LifeSure_DbFirst.Dtos.ContactMessageDtos;
using Mvc_LifeSure_DbFirst.Services.ContactMessageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactMessageService _contactService;

        public ContactController(IContactMessageService contactService)
        {
            _contactService = contactService;
        }

        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMessage(CreateContactMessageDto createDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _contactService.Create(createDto);
                    TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Mesaj gönderilirken bir hata oluştu: " + ex.Message);
                }
            }

            return View("Index", createDto);
        }
    }
}