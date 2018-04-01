using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnovaStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private readonly IClaimPrincipalManager _claimPrincipalManager;

        public HomeController(IClaimPrincipalManager claimPrincipalManager)
        {
            _claimPrincipalManager = claimPrincipalManager;
        }

        [Authorize(Policy = "Staff Only")]
        [HttpGet]
        public IActionResult Index()
        {
            switch (_claimPrincipalManager.Role)
            {
                case "CustomerSupport":
                    return RedirectToAction("Index", "Support", new {Area = "Manage"});

                case "Administrator":
                    return RedirectToAction("Index", "Admin", new {Area = "Manage"});

                case "ProductManager":
                    return RedirectToAction("Index", "Manager", new { Area = "Manage" });

                case "Shipper":
                    return RedirectToAction("Index", "Shipper", new { Area = "Manage" });
                default:
                    return RedirectToAction("Index", "Home", new {Area = ""});
            }
        }
    }
}