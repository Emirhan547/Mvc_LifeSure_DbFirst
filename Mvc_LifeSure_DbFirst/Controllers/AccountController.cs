using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAdminLogService _logService;

        // Constructor'da sadece kendi servislerimizi alalım
        public AccountController(

            IAdminLogService logService)
        {

            _logService = logService;
        }

        // UserManager'ı OWIN'den al
        private AppUserManager _userManager;
        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // SignInManager'ı OWIN'den al
        private AppSignInManager _signInManager;
        public AppSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Default");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginDto model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user != null && !user.IsActive)
            {
                ModelState.AddModelError("", "Hesabınız pasif durumda. Lütfen yönetici ile iletişime geçin.");
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    // Loglama
                    _logService.LogAdminAction(
                        user?.Id,
                        $"{model.UserName} kullanıcısı giriş yaptı",
                        "Login",
                        "Account"
                    );
                    return await RedirectToLocalAsync(returnUrl, user);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Geçersiz giriş denemesi.");
                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Default");
            }

            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    City = model.City,
                    BirthDate = model.BirthDate,
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Default role atama (User)
                    await UserManager.AddToRoleAsync(user.Id, "User");

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // Loglama
                    _logService.LogAdminAction(
                        user.Id,
                        $"{user.UserName} kullanıcısı kayıt oldu",
                        "Register",
                        "Account"
                    );

                    return RedirectToAction("Index", "Default");
                }

                AddErrors(result);
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            var userId = User.Identity.GetUserId();
            var userName = User.Identity.Name;

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Loglama
            if (!string.IsNullOrEmpty(userId))
            {
                _logService.LogAdminAction(
                    userId,
                    $"{userName} kullanıcısı çıkış yaptı",
                    "Logout",
                    "Account"
                );
            }

            return RedirectToAction("Index", "Default");
        }

        // GET: /Account/Profile


        private async Task<ActionResult> RedirectToLocalAsync(string returnUrl, AppUser user)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            if (user != null && await UserManager.IsInRoleAsync(user.Id, "Admin"))
            {
                return RedirectToAction("Index", "About", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Default");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

    }
}

           