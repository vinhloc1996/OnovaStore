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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnovaStore.Areas.Manage.Data;
using OnovaStore.Models.Order;
using Stripe;

namespace OnovaStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CheckoutController : Controller
    {
        private readonly IClaimPrincipalManager _claimPrincipalManager;
        private readonly IRestClient _restClient;

        public CheckoutController(IClaimPrincipalManager claimPrincipalManager, IRestClient restClient)
        {
            _claimPrincipalManager = claimPrincipalManager;
            _restClient = restClient;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string error)
        {
            if (_claimPrincipalManager.IsAuthenticated)
            {
                using (var client = _restClient.CreateClient(User))
                {
                    using (var response = await client.GetAsync("/api/customercart/GetCustomerCarts?customerId=" + _claimPrincipalManager.Id + "&error=" + error))
                    {
                        dynamic result = response.StatusCode == HttpStatusCode.OK
                            ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                            : null;

                        return View(result);
                    }
                }
            }

            var customerId = Request.Cookies["AnonymousId"];
            if (string.IsNullOrEmpty(customerId))
            {
                return View(null);
            }
            else
            {
                using (var client = _restClient.CreateClient(User))
                {
                    using (var response = await client.GetAsync("/api/anonymouscustomercart/GetAnonymousCustomerCarts?customerId=" + customerId))
                    {
                        dynamic result = response.StatusCode == HttpStatusCode.OK
                            ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                            : null;

                        return View(result);
                    }
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Charge(string stripeEmail, string stripeToken, string stripeBillingName, string stripeBillingAddressLine1, string stripeBillingAddressZip, string stripeBillingAddressState, string stripeBillingAddressCity, int totalPrice)
        {
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();
            string anonymousId = Request.Cookies["AnonymousId"];

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = totalPrice,
                Description = "Payment Order",
                Currency = "usd",
                CustomerId = customer.Id,
                ReceiptEmail = stripeEmail
            });

            

            if (charge.Paid)
            {
                if (_claimPrincipalManager.IsAuthenticated)
                {
                    var order = new Order
                    {
                        TypeUser = "customer",
                        CartId = _claimPrincipalManager.Id,
                        TokenId = charge.Id,
                        TotalPrice = (double) totalPrice/100
                    };

                    using (var client = _restClient.CreateClient(User))
                    {
                        using (var response = await client.PostAsync("/api/order/createorder", new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8,
                            "application/json")))
                        {
                            dynamic result = response.StatusCode == HttpStatusCode.OK
                                ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                                : null;

                            if (result.status == "Success")
                            {
                                return View("Success", result.orderCode.ToString());
                            }

                            return RedirectToAction("Index", "Checkout", new {error = result.message.ToString() });
                        }
                    }
                }

                if (string.IsNullOrEmpty(anonymousId))
                {
                    return RedirectToAction("Index", "Checkout", new { error = "Invalid customer id" });
                }

                var anonymousOrder = new Order
                {
                    Email = stripeEmail,
                    AddressLine1 = stripeBillingAddressLine1,
//                    AddressLine2 = "",
                    CartId = anonymousId,
                    TypeUser = "anonymous",
                    City = stripeBillingAddressCity,
                    Zip = stripeBillingAddressZip,
                    Phone = "None",
                    FullName = stripeBillingName,
                    TokenId = charge.Id,
                    TotalPrice = (double)totalPrice / 100
                };

                using (var client = _restClient.CreateClient(User))
                {
                    using (var response = await client.PostAsync("/api/order/createorder", new StringContent(JsonConvert.SerializeObject(anonymousOrder), Encoding.UTF8,
                        "application/json")))
                    {
                        dynamic result = response.StatusCode == HttpStatusCode.OK
                            ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                            : null;

                        if (result.status == "Success")
                        {
                            return View("Success", result.orderCode.ToString());
                        }

                        return RedirectToAction("Index", "Checkout", new { error = result.message.ToString() });
                    }
                }
            }

            return View("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Success([FromQuery] string orderCode)
        {
            
            return View();
        }
    }
}