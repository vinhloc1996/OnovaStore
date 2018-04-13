using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OnovaStore.Controllers
{
    public class CartController : Controller
    {
        [HttpGet]
        public IActionResult AddToCart([FromQuery] int productId)
        {
            Set("Cart", productId+",");

            return Ok();
        }

        public void Set(string key, string value)
        {
            CookieOptions option = new CookieOptions();

            option.Expires = DateTime.Now.AddDays(30);

            Response.Cookies.Append(key, value, option);
        }
    }
}