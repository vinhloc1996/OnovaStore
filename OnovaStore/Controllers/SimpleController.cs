using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnovaStore.Controllers
{
    public class SimpleController : Controller
    {
        public IActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoginViaFacebook([FromQuery] string access_token, [FromQuery] string error, [FromQuery] string error_description)
        {
            
            if (!string.IsNullOrEmpty(access_token))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5000");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = client.GetAsync($"api/auth/facebooklogin?access_token={access_token}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return View();
        }
    }
}