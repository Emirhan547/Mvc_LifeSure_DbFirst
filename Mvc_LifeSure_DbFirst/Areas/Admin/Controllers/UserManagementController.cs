using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Data.Identity;
using Mvc_LifeSure_DbFirst.Dtos.AppUserDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.AppUserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class UserManagementController : AdminBaseController
    {
        private readonly IAppUserService _userService;
        private AppUserManager _userManager;

        public UserManagementController(
            IAdminLogService logService,
            IAppUserService userService,
            AppUserManager userManager) : base(logService)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public AppUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
            private set { _userManager = value; }
        }

        public async Task<ActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var user = await _userService.GetUserWithPoliciesAsync(id);
            if (user == null)
                return HttpNotFound();

            // Kullanıcının rollerini getir
            var appUser = await UserManager.FindByIdAsync(id);
            var roles = await UserManager.GetRolesAsync(id);
            ViewBag.UserRoles = roles;

            return View(user);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            var updateDto = new UpdateAppUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateAppUserDto updateDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUserAsync(updateDto);

                    LogAction(
                        $"{updateDto.UserName} kullanıcısı güncellendi",
                        "Update",
                        "Users",
                        int.TryParse(updateDto.Id, out int id) ? id : 0
                    );

                    TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(updateDto);
        }

        [HttpPost]
        public async Task<ActionResult> ToggleStatus(string id)
        {
            try
            {
                var isActive = await _userService.ToggleUserStatusAsync(id);
                var user = await _userService.GetUserByIdAsync(id);

                LogAction(
                    $"{user.UserName} kullanıcısı {(isActive ? "aktifleştirildi" : "pasifleştirildi")}",
                    "Update",
                    "Users",
                    int.TryParse(id, out int userId) ? userId : 0
                );

                return Json(new { success = true, isActive = isActive });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                await _userService.DeleteUserAsync(id);

                LogAction(
                    $"{user.UserName} kullanıcısı silindi",
                    "Delete",
                    "Users",
                    int.TryParse(id, out int userId) ? userId : 0
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<ActionResult> ManageRoles(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            var userRoles = await UserManager.GetRolesAsync(id);
            var allRoles = new List<string> { "Admin", "User", "Agent" }; // Rolleri buradan alabilirsin

            ViewBag.UserId = id;
            ViewBag.UserName = user.UserName;
            ViewBag.UserRoles = userRoles;
            ViewBag.AllRoles = allRoles;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(string userId, string role)
        {
            try
            {
                var result = await UserManager.AddToRoleAsync(userId, role);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(userId);
                    LogAction(
                        $"{user.UserName} kullanıcısına {role} rolü eklendi",
                        "Update",
                        "UserRoles",
                        int.TryParse(userId, out int id) ? id : 0
                    );

                    return Json(new { success = true });
                }
                return Json(new { success = false, message = string.Join(", ", result.Errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveRole(string userId, string role)
        {
            try
            {
                var result = await UserManager.RemoveFromRoleAsync(userId, role);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(userId);
                    LogAction(
                        $"{user.UserName} kullanıcısından {role} rolü çıkarıldı",
                        "Update",
                        "UserRoles",
                        int.TryParse(userId, out int id) ? id : 0
                    );

                    return Json(new { success = true });
                }
                return Json(new { success = false, message = string.Join(", ", result.Errors) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}