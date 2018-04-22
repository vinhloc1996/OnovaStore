using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnovaStore.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Detail([FromRoute] string slug)
        {
            return View();
        }
    }
}