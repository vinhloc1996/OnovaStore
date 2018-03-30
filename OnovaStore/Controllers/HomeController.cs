using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnovaStore.Models.Brand;

namespace OnovaStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private readonly IRestClient restClient;

        public HomeController(IRestClient restClient)
        {
            this.restClient = restClient;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    
        //test role function
        [Authorize(Roles = "Administrator, CustomerSupport")]
        [HttpGet]
        public async Task<IActionResult> Success()
        {
            using (var client = restClient.CreateClient(User))
            {
                using (var response = await client.GetAsync("/api/brand"))
                {

                    dynamic result = response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : String.Empty;

                    var list = JsonConvert.DeserializeObject<List<Brand>>(result);

                    return View(list);
                }
            }
            
        }
    }
}