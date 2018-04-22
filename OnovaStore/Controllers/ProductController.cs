using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OnovaStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IClaimPrincipalManager _claimPrincipalManager;
        private readonly IRestClient _restClient;

        public ProductController(IClaimPrincipalManager claimPrincipalManager, IRestClient restClient)
        {
            _claimPrincipalManager = claimPrincipalManager;
            _restClient = restClient;
        }

        public async Task<IActionResult> Detail([FromRoute] string slug)
        {
            //split productId
            if (slug != null)
            {
                string[] id = slug.Split('-');
                string productId = id[id.Length - 1].Substring(1);

                using (var client = _restClient.CreateClient(User))
                {
                    using (var response = await client.GetAsync("/api/product/GetProduct?id=" + productId))
                    {
                        dynamic result = response.StatusCode == HttpStatusCode.OK
                            ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                            : null;

                        if (result != null && result.status == "Success")
                        {
                            return View(result.product[0]);
                        }
                    }
                }
            }

            //Return View 404 not found
            return RedirectToAction("NotFound404", "Home");
        }

        public async Task<IActionResult> SearchProduct(string keyword)
        {
            return View();
        }
    }
}