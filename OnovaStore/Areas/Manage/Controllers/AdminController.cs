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
using OnovaStore.Areas.Manage.Models.Admin;
using OnovaStore.Helpers;

namespace OnovaStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IClaimPrincipalManager _claimPrincipalManager;
        private readonly IRestClient _restClient;

        public AdminController(IClaimPrincipalManager claimPrincipalManager, IRestClient restClient)
        {
            _claimPrincipalManager = claimPrincipalManager;
            _restClient = restClient;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
            int? page = 1)
        {
            var staffs = new List<GetStaffsForAdmin>();

            if (searchString == null)
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            var sortQuery = new List<string>
            {
                "name",
                "name_desc",
                "adddate",
                "adddate_desc",
                "salary",
                "salary_desc",
                "email",
                "email_desc",
                "role",
                "role_desc",
            };

            sortOrder = string.IsNullOrEmpty(sortOrder) || !sortQuery.Contains(sortOrder)
                ? "name"
                : sortOrder.Trim().ToLower();

            var queryString = nameof(sortOrder) + "=" + sortOrder + (!string.IsNullOrEmpty(searchString)
                                  ? "&" + nameof(searchString) + "=" + searchString
                                  : "");

            ViewData["SortOrder"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.GetAsync("/api/admin/GetStaffsForAdmin?" + queryString))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        staffs = JsonConvert.DeserializeObject<List<GetStaffsForAdmin>>(
                            await response.Content.ReadAsStringAsync());
                    }
                }
            }

            int pageSize = 5;
            ViewData["LengthEntry"] = staffs.Count;
            ViewData["CurrentEntry"] = pageSize * page;

            return View(PaginatedList<GetStaffsForAdmin>.CreateAsync(staffs, page ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> AddStaff()
        {
            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.GetAsync("/api/admin/GetRoles"))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ViewData["Roles"] = JsonConvert.DeserializeObject<List<dynamic>>(
                            await response.Content.ReadAsStringAsync());
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStaff(AddStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var client = _restClient.CreateClient(User))
                {
                    using (
                        var response = await client.PostAsync("/api/auth/AddStaff",
                            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                                "application/json")))
                    {
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }

            ModelState.AddModelError("AddFail", "Add Staff failed please refresh page");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.PostAsync("/api/auth/ResetStaffPassword", new StringContent(JsonConvert.SerializeObject(new
                        {
                            id
                        }), Encoding.UTF8,
                        "application/json")))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ViewData["Success"] = response.Content.ReadAsStringAsync();
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}