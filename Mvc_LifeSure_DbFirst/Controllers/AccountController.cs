using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Data.Identity;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.AppUserServices;
using Mvc_LifeSure_DbFirst.ViewModels;
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
        private readonly IAppUserService _userService;
        private readonly IAdminLogService _logService;
        private AppUserManager _userManager;
        private AppSignInManager _signInManager;

        public AccountController(
            IAppUserService userService,
            IAdminLogService logService,
            AppUserManager userManager,
            AppSignInManager signInManager)
        {
            _userService = userService;
            _logService = logService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public AppUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
            private set { _userManager = value; }
        }

        public AppSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>(); }
            private set { _signInManager = value; }
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
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
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
                    return RedirectToLocal(returnUrl);

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
        public async Task<ActionResult> Register(RegisterViewModel model)
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
        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var userId = User.Identity.GetUserId();
            var user = await _userService.GetUserWithPoliciesAsync(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            var profile = new ProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                PolicyCount = user.PolicyCount
            };

            return View(profile);
        }

        // GET: /Account/EditProfile
        [Authorize]
        public async Task<ActionResult> EditProfile()
        {
            var userId = User.Identity.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new ProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                BirthDate = user.BirthDate,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        // POST: /Account/EditProfile
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = await UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return HttpNotFound();
                }

                // Kullanıcı bilgilerini güncelle
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.City = model.City;
                user.BirthDate = model.BirthDate;
                user.PhoneNumber = model.PhoneNumber;

                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Loglama
                    _logService.LogAdminAction(
                        userId,
                        $"{user.UserName} kullanıcısı profilini güncelledi",
                        "Update",
                        "Account",
                        int.TryParse(userId, out int id) ? id : 0
                    );

                    return RedirectToAction("Profile");
                }

                AddErrors(result);
            }

            return View(model);
        }

        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity.GetUserId();
            var result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // Loglama
                    _logService.LogAdminAction(
                        userId,
                        $"{user.UserName} kullanıcısı şifresini değiştirdi",
                        "ChangePassword",
                        "Account"
                    );
                }

                return RedirectToAction("Profile", new { Message = "Şifreniz başarıyla değiştirildi." });
            }

            AddErrors(result);
            return View(model);
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Kullanıcı bulunamadı veya email onaylı değil
                    return View("ForgotPasswordConfirmation");
                }

                // TODO: Email gönderme işlemi
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                // await UserManager.SendEmailAsync(user.Id, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için <a href='{callbackUrl}'>tıklayınız</a>");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Kullanıcı bulunamadı
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                // Loglama
                _logService.LogAdminAction(
                    user.Id,
                    $"{user.UserName} kullanıcısı şifresini sıfırladı",
                    "ResetPassword",
                    "Account"
                );

                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/AccessDenied
        public ActionResult AccessDenied()
        {
            return View();
        }

        #region Yardımcı Metotlar
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
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
        #endregion
    }
}