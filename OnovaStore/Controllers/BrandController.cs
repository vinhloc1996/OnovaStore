using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnovaStore.Helpers;
using OnovaStore.Models.Brand;

namespace OnovaStore.Controllers
{
    public class BrandController : Controller
    {
        private readonly IClaimPrincipalManager _claimPrincipalManager;
        private readonly IRestClient _restClient;

        public BrandController(IClaimPrincipalManager claimPrincipalManager, IRestClient restClient)
        {
            _claimPrincipalManager = claimPrincipalManager;
            _restClient = restClient;
        }

        public async Task<IActionResult> List([FromRoute] string slug, string sortOrder,
            int? page = 1)
        {
            if (slug != null)
            {
                string[] id = slug.Split('-');
                string brandId = id[id.Length - 1].Substring(1);

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

                var queryString = nameof(sortOrder) + "=" + sortOrder;

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
                ViewData["Slug"] = slug;

                int pageSize = 8;

                using (var client = _restClient.CreateClient(User))
                {
                    using (var response = await client.GetAsync("/api/brand/GetProductsForBrand?id=" + brandId + "&" + queryString))
                    {
                        RootBrandProducts root = response.StatusCode == HttpStatusCode.OK
                            ? JsonConvert.DeserializeObject<RootBrandProducts>(await response.Content.ReadAsStringAsync())
                            : null;

                        if (root != null && root.status == "Success")
                        {
//                            ViewData["LengthEntry"] = root.brands.products.Count;
//                            ViewData["CurrentEntry"] = pageSize * page;
                            ViewData["CurrentPage"] = page;
                            ViewBag.BrandInfo = root.brands;
                            return View(PaginatedList<BrandProducts>.CreateAsync(root.brands.products, page ?? 1, pageSize));
                        }

                        return View(null);
                    }
                }
            }

            //Return View 404 not found
            return RedirectToAction("NotFound404", "Home");
        }
    }
}