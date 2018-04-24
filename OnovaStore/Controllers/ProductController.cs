using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnovaStore.Helpers;
using OnovaStore.Models.Product;

namespace OnovaStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        public async Task<IActionResult> SearchProduct(string sortOrder, string keyword,
            int? page = 1)
        {
            var sortQuery = new List<string>
            {
                "name",
                "name_desc",
                "price",
                "price_desc"
            };

            sortOrder = string.IsNullOrEmpty(sortOrder) || !sortQuery.Contains(sortOrder)
                ? "name"
                : sortOrder.Trim().ToLower();

            var queryString = nameof(sortOrder) + "=" + sortOrder + (!string.IsNullOrEmpty(keyword)
                                  ? "&" + nameof(keyword) + "=" + keyword
                                  : "");

            string displayOrder = "";

            switch (sortOrder)
            {
                case "":
                case "name":
                    displayOrder = "Name A-Z";
                    break;
                case "name_desc":
                    displayOrder = "Name Z-A";
                    break;
                case "price":
                    displayOrder = "Price low to high";
                    break;
                case "price_desc":
                    displayOrder = "Price high to low";
                    break;
            }

            ViewData["DisplayOrder"] = displayOrder;
            ViewData["SortOrder"] = sortOrder;
            ViewData["KeyWord"] = keyword;
            int pageSize = 8;

            using (var client = _restClient.CreateClient(User))
            {
                using (var response = await client.GetAsync("/api/product/SearchProduct?" + queryString))
                {
                    RootProductSearch root = response.StatusCode == HttpStatusCode.OK
                        ? JsonConvert.DeserializeObject<RootProductSearch>(await response.Content.ReadAsStringAsync())
                        : null;

                    if (root != null && root.status == "Success")
                    {
//                        ViewData["LengthEntry"] = root.products.Count;
//                        ViewData["CurrentEntry"] = pageSize * page;
                        ViewData["CurrentPage"] = page;

                        return View(PaginatedList<ProductSearch>.CreateAsync(root.products, page ?? 1, pageSize));
                    }

                    return View(null);
                }
            }
        }
    }
}