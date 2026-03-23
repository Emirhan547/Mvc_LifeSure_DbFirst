using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Data.Identity;
using Mvc_LifeSure_DbFirst.Dtos.AppUserDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.AppUserServices;
using System;
using System.Collections.Generic;
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

        private AppUserManager UserManager =>
            _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

        // GET: Admin/UserManagement
        public async Task<ActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        // GET: Admin/UserManagement/Details/id
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            try
            {
                var user = await _userService.GetUserWithPoliciesAsync(id);
                if (user == null) return HttpNotFound();

                var roles = await UserManager.GetRolesAsync(id);
                ViewBag.UserRoles = roles;
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/UserManagement/Edit/id
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return HttpNotFound();

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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/UserManagement/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UpdateAppUserDto updateDto)
        {
            if (!ModelState.IsValid)
                return View(updateDto);

            try
            {
                await _userService.UpdateUserAsync(updateDto);
                LogAction($"{updateDto.UserName} kullanıcısı güncellendi", "Update", "Users");
                TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(updateDto);
            }
        }

        // POST: Admin/UserManagement/ToggleStatus  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "Geçersiz kullanıcı kimliği." });

            try
            {
                var isActive = await _userService.ToggleUserStatusAsync(id);
                var user = await _userService.GetUserByIdAsync(id);
                var statusText = isActive ? "aktifleştirildi" : "pasifleştirildi";
                LogAction($"{user?.UserName} kullanıcısı {statusText}", "Update", "Users");
                return Json(new { success = true, isActive });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/UserManagement/Delete  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Json(new { success = false, message = "Geçersiz kullanıcı kimliği." });

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                await _userService.DeleteUserAsync(id);
                LogAction($"{user?.UserName} kullanıcısı silindi", "Delete", "Users");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Admin/UserManagement/ManageRoles/id
        public async Task<ActionResult> ManageRoles(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return HttpNotFound();

            var userRoles = await UserManager.GetRolesAsync(id);
            ViewBag.UserId = id;
            ViewBag.UserName = user.UserName;
            ViewBag.UserRoles = userRoles;
            ViewBag.AllRoles = new List<string> { "Admin", "User", "Agent" };
            return View();
        }

        // POST: Admin/UserManagement/AddRole  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
                return Json(new { success = false, message = "Geçersiz parametre." });

            try
            {
                var result = await UserManager.AddToRoleAsync(userId, role);
                if (!result.Succeeded)
                    return Json(new { success = false, message = string.Join(", ", result.Errors) });

                var user = await UserManager.FindByIdAsync(userId);
                LogAction($"{user?.UserName} kullanıcısına {role} rolü eklendi", "Update", "UserRoles");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Admin/UserManagement/RemoveRole  (AJAX)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveRole(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
                return Json(new { success = false, message = "Geçersiz parametre." });

            try
            {
                var result = await UserManager.RemoveFromRoleAsync(userId, role);
                if (!result.Succeeded)
                    return Json(new { success = false, message = string.Join(", ", result.Errors) });

                var user = await UserManager.FindByIdAsync(userId);
                LogAction($"{user?.UserName} kullanıcısından {role} rolü kaldırıldı", "Update", "UserRoles");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
