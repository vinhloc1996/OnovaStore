using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnovaStore.Models.Account;

namespace OnovaStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly IClaimPrincipalManager claimPrincipalManager;

        public AccountController(IClaimPrincipalManager claimPrincipalManager)
        {
            this.claimPrincipalManager = claimPrincipalManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(String returnUrl = null)
        {
            if (claimPrincipalManager.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await claimPrincipalManager.LogoutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model, String returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(String.Empty, "Invalid login form");
                return View(model);
            }

            if (await claimPrincipalManager.LoginAsync(model.Email, model.Password))
                return RedirectToLocal(returnUrl);

            ModelState.AddModelError(String.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(String returnUrl = null)
        {
            if (claimPrincipalManager.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        private IActionResult RedirectToLocal(String returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}