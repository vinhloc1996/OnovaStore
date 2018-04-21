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
using OnovaStore.Areas.Manage.Data;
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
        public async Task<IActionResult> Index()
        {
            if (_claimPrincipalManager.IsAuthenticated)
            {
                using (var client = _restClient.CreateClient(User))
                {
                    using (var response = await client.GetAsync("/api/customercart/GetCustomerCarts?customerId=" + _claimPrincipalManager.Id))
                    {
                        dynamic result = response.StatusCode == HttpStatusCode.OK
                            ? JsonConvert.DeserializeObject<List<dynamic>>(await response.Content.ReadAsStringAsync())
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
                            ? JsonConvert.DeserializeObject<List<dynamic>>(await response.Content.ReadAsStringAsync())
                            : null;

                        return View(result);
                    }
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Charge(string stripeEmail, string stripeToken, string submitAddressLine1, string submitAddressLine2, string submitCity, string submitPhone, string submitFullName, string submitZipCode, string submitEmail, int totalPrice, string anonymousId)
        {
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

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
                CustomerId = customer.Id
            });

            return View();
        }


    }
}