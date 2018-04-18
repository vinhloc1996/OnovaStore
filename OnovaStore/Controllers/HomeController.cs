using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnovaStore.Models.Brand;
using OnovaStore.Models.Category;

namespace OnovaStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HomeController : Controller
    {
        private readonly IRestClient restClient;

        private readonly IClaimPrincipalManager _claimPrincipalManager;

        public HomeController(IRestClient restClient, IClaimPrincipalManager claimPrincipalManager)
        {
            this.restClient = restClient;
            _claimPrincipalManager = claimPrincipalManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var meetPolicy = await _claimPrincipalManager.HasPolicy("Staff Only");
            if (meetPolicy)
            {
                return RedirectToAction("Index", "Home", new {area = "Manage"});
            }
            
            using (var client = restClient.CreateClient(User))
            {
                using (var response = await client.GetAsync("/api/category/GetCategoriesForHeader"))
                {
                    dynamic result = response.StatusCode == HttpStatusCode.OK
                        ? await response.Content.ReadAsStringAsync()
                        : null;

                    if (result != null)
                    {
                        var list = JsonConvert.DeserializeObject<List<Category>>(result);
                        ViewBag.HeaderCategories = list;
                    }
                }
            }

            using (var client = restClient.CreateClient(User))
            {
                using (var response = await client.GetAsync("/api/brand/GetBrandsForHeader"))
                {
                    dynamic result = response.StatusCode == HttpStatusCode.OK
                        ? await response.Content.ReadAsStringAsync()
                        : null;

                    if (result != null)
                    {
                        var list = JsonConvert.DeserializeObject<List<Brand>>(result);
                        ViewBag.HeaderBrands = list;
                    }

                }
            }

            using (var client = restClient.CreateClient(User))
            {
                using (var response = await client.GetAsync("/api/category/GetCategoriesForIndexPage"))
                {
                    dynamic result = response.StatusCode == HttpStatusCode.OK
                        ? await response.Content.ReadAsStringAsync()
                        : null;

                    if (result != null)
                    {
                        var list = JsonConvert.DeserializeObject<List<Category>>(result);
                        ViewBag.CategoryProducts = list;
                    }
                }
            }

            return View();
        }

        //test role function
//        [Authorize(Roles = "Administrator, CustomerSupport")]
//        [HttpGet]
//        public async Task<IActionResult> Success()
//        {
//            using (var client = restClient.CreateClient(User))
//            {
//                using (var response = await client.GetAsync("/api/brand"))
//                {
//
//                    dynamic result = response.StatusCode == HttpStatusCode.OK ? await response.Content.ReadAsStringAsync() : String.Empty;
//
//                    var list = JsonConvert.DeserializeObject<List<>>(result);
//
//                    return View(list);
//                }
//            }
//            
//        }
    }
}