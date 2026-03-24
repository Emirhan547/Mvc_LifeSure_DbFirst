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
        private static readonly List<string> AvailableRoles = new List<string> { "Admin", "User", "Agent" };
        private readonly IAppUserService _userService;


        public UserManagementController(IAdminLogService logService, IAppUserService userService) : base(logService)
        {
            _userService = userService;
          
        }

        
        public async Task<ActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return HttpNotFound();

            try
            {
                var user = await _userService.GetUserWithPoliciesAsync(id);
                ViewBag.UserRoles = await _userService.GetUserRolesAsync(id);
              
                return View(user);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

       
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return HttpNotFound();

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
               

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

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ToggleStatus(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
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

    
        public async Task<ActionResult> ManageRoles(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return HttpNotFound();

            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                ViewBag.UserId = id;
                ViewBag.UserName = user.UserName;
                ViewBag.UserRoles = await _userService.GetUserRolesAsync(id);
                ViewBag.AllRoles = AvailableRoles;
                return View();
            }
            catch
            {
                return HttpNotFound();
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRole(string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
                return Json(new { success = false, message = "Geçersiz parametre." });

            try
            {
                await _userService.AddUserToRoleAsync(userId, role);
                var user = await _userService.GetUserByIdAsync(userId);
                LogAction($"{user?.UserName} kullanıcısına {role} rolü eklendi", "Update", "UserRoles");
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveRole(string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
                return Json(new { success = false, message = "Geçersiz parametre." });

            try
            {
                await _userService.RemoveUserFromRoleAsync(userId, role);
                var user = await _userService.GetUserByIdAsync(userId);
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
