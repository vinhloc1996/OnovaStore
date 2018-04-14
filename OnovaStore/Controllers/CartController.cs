using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnovaStore.Models.Brand;
using OnovaStore.Models.Category;
using OnovaStore.Models.CustomerCart;
using OnovaStore.Models.Product;

namespace OnovaStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartController : Controller
    {
        private readonly IRestClient restClient;

        private readonly IClaimPrincipalManager _claimPrincipalManager;

        public CartController(IRestClient restClient, IClaimPrincipalManager claimPrincipalManager)
        {
            this.restClient = restClient;
            _claimPrincipalManager = claimPrincipalManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AddToCart([FromQuery] int productId)
        {
            if (_claimPrincipalManager.IsAuthenticated)
            {
                using (var client = restClient.CreateClient(User))
                {
                    using (var response = await client.GetAsync("/api/product/" + productId))
                    {
                        string result = response.StatusCode == HttpStatusCode.OK
                            ? await response.Content.ReadAsStringAsync()
                            : null;

                        if (result != null)
                        {
                            var product = JsonConvert.DeserializeObject<ProductCart>(result);

                            var customerCartDetail = new CustomerCartDetail
                            {
                                ProductId = product.ProductId,
                                CustomerCartId = _claimPrincipalManager.Id,
                                Price = product.RealPrice,
                                DisplayPrice = product.DisplayPrice,
                                Quantity = product.CurrentQuantity
                            };

                            using (var addCartDetail = await client.PostAsync("/api/CustomerCartDetail/",
                                new StringContent(JsonConvert.SerializeObject(customerCartDetail), Encoding.UTF8,
                                    "application/json")))
                            {
                                if (addCartDetail.IsSuccessStatusCode)
                                {
                                    using (var getCartDetail = await client.GetAsync("/api/CustomerCartDetail/" + customerCartDetail.CustomerCartId))
                                    {
                                        string listCart = response.StatusCode == HttpStatusCode.OK
                                            ? await response.Content.ReadAsStringAsync()
                                            : null;

//                                        var cartDetail = JsonConvert.DeserializeObject<List<dynamic>>(listCart);

                                        return Json(listCart);
                                    }
                                }
                            }
                        }

                    }
                }
            }

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