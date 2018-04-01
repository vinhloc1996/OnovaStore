using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnovaStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListReview()
        {
            return View();
        }

        public IActionResult ListQuestion()
        {
            return View();
        }
    }
}