using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnovaStore.Models.Account;

namespace OnovaStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IClaimPrincipalManager claimPrincipalManager;

        public AccountController(IClaimPrincipalManager claimPrincipalManager)
        {
            this.claimPrincipalManager = claimPrincipalManager;
        }

        [HttpGet]
        public IActionResult Login(String returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await claimPrincipalManager.LogoutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
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

        private IActionResult RedirectToLocal(String returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}