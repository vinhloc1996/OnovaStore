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
using OnovaStore.Helpers;
using OnovaStore.Models.Account;
using OnovaStore.Models.Category;

namespace OnovaStore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountSettingController : Controller
    {

        private readonly IClaimPrincipalManager _claimPrincipalManager;
        private readonly IRestClient _restClient;

        public AccountSettingController(IClaimPrincipalManager claimPrincipalManager, IRestClient restClient)
        {
            _claimPrincipalManager = claimPrincipalManager;
            _restClient = restClient;
        }

        [HttpGet]
        public async Task<IActionResult> UserInformation()
        {
            using (var client = _restClient.CreateClient(User))
            {
                using (var response = await client.GetAsync("/api/customer/GetCustomerInfo"))
                {
                    dynamic root = response.StatusCode == HttpStatusCode.OK
                        ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                        : null;

                    if (root != null)
                    {
                        return View(root);
                    }
                }
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> UserInformation(string fullname, string gender, string dob)
        {
            var a = DateTime.Parse(dob);

            using (var client = _restClient.CreateClient(User))
            {
                using (var response = await client.PostAsync("/api/customer/UpdateCustomerInfo", new StringContent(JsonConvert.SerializeObject(new {fullname, gender, dob}), Encoding.UTF8,
                    "application/json")))
                {
                    dynamic root = response.StatusCode == HttpStatusCode.OK
                        ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                        : null;

                    if (root != null && root.status == "Success")
                    {
                        return View(root.customer);
                    }
                }
            }

            return await UserInformation();
        }

        [HttpGet]
        public async Task<IActionResult> Order()
        {
            using (var client = _restClient.CreateClient(User))
            {
                using (var response = await client.GetAsync("/api/order/ShowOrdersForUser"))
                {
                    dynamic root = response.StatusCode == HttpStatusCode.OK
                        ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                        : null;

                    if (root != null)
                    {
                        return View(root);
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = _restClient.CreateClient(User))
                {
                    using (var response = await client.PostAsync("/api/customer/ChangePassword", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                        "application/json")))
                    {
                        dynamic root = response.StatusCode == HttpStatusCode.OK
                            ? JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync())
                            : null;

                        if (root != null && root.status == "Success")
                        {
                            ModelState.AddModelError("ChangePasswordSuccess", "Change password successful");
                            return View();
                        }

                        ModelState.AddModelError("ChangePasswordFailed", "Change password failed");
                        return View(model);
                    }
                }
            }

            
            return View(model);
        }
    }
}