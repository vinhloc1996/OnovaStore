using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnovaStore.Areas.Manage.Data;
using OnovaStore.Areas.Manage.Models.Brand;
using OnovaStore.Areas.Manage.Models.Image;
using OnovaStore.Helpers;
using OnovaStore.Models.Brand;

namespace OnovaStore.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize]
//    [Authorize(Roles = "ProductManager")]
    public class ManagerController : Controller
    {
        private readonly IClaimPrincipalManager _claimPrincipalManager;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private readonly IRestClient _restClient;
        private Cloudinary _cloudinary;

        public ManagerController(IRestClient restClient, IClaimPrincipalManager claimPrincipalManager,
            IOptions<CloudinarySettings> cloudinarySettings)
        {
            _restClient = restClient;
            _cloudinarySettings = cloudinarySettings;
            _claimPrincipalManager = claimPrincipalManager;

            Account acc = new Account(
                _cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(UploadImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var size = model.Files.Sum(e => e.Length);

                if (size > 5242880)
                {
                    ModelState.AddModelError(string.Empty, "File Size is not greater than 5MB");
                    return View(model);
                }

                var array = await UploadImages(model.Files);

                return View();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Products()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(object model)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Brands(string sortOrder)
        {
            var brands = new List<GetBrandForStaff>();

            var sortQuery = new List<string>
            {
                "id", "id_desc", "name", "name_desc", "totalproduct", "totalproduct_desc", "totalsale", "totalsale_desc", "rate", "rate_desc"
            };

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "id" : sortOrder.Trim().ToLower();

            if (sortQuery.Contains(sortOrder))
            {
                ViewData["SortOrder"] = sortOrder;
                using (var client = _restClient.CreateClient(User))
                {
                    using (
                        var response = await client.GetAsync("/api/brand/GetBrandsForStaff"))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            brands = JsonConvert.DeserializeObject<List<GetBrandForStaff>>(await response.Content.ReadAsStringAsync());
                        }
                    }
                }
            }
            
            return View(brands);
        }

        [HttpGet]
        public IActionResult AddBrand()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBrand(AddBrandViewModel model)
        {
//            ModelState.AddModelError("keyName", "Form is not valid");

            if (ModelState.IsValid)
            {
                var size = model.BrandImage.Length;

                if (size > 5242880)
                {
                    ModelState.AddModelError("OverLength", "File Size is not greater than 5MB");
                    return View(model);
                }

                var imageId = await UploadImages(new List<IFormFile>
                {
                    model.BrandImage
                });

                var brand = new AddBrandDTO
                {
                    BrandImage = imageId[0],
                    Slug = model.Name.URLFriendly(),
                    ContactEmail = model.ContactEmail,
                    ContactName = model.ContactName,
                    ContactPhone = model.ContactPhone,
                    ContactTitle = model.ContactTitle,
                    Name = model.Name
                };

                using (var client = _restClient.CreateClient(User))
                {
                    using (
                        var response = await client.PostAsync("/api/brand",
                            new StringContent(JsonConvert.SerializeObject(brand), Encoding.UTF8,
                                "application/json")))
                    {
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            return View("Brands");
                        }
                        
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EditBrand([FromRoute] int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Categories()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(object model)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Promotions()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddPromotion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPromotion(object model)
        {
            return View();
        }

        private async Task<List<string>> UploadImages(List<IFormFile> images)
        {
            var staffId = _claimPrincipalManager.Id;
            var uploadResult = new ImageUploadResult();
            var result = new List<string>();

            if (images.Count > 0)
            {
                foreach (var file in images)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream)
                        };

                        uploadResult = _cloudinary.Upload(uploadParams);

                        if (uploadResult.StatusCode == HttpStatusCode.OK)
                        {
                            var photoDto = new ImageUploadDTO
                            {
                                StaffId = staffId,
                                ImageUrl = uploadResult.Uri.ToString(),
                                ImageId = uploadResult.PublicId
                            };

                            using (var client = _restClient.CreateClient(User))
                            {
                                using (
                                    var response = await client.PostAsync("/api/GeneralImage",
                                        new StringContent(JsonConvert.SerializeObject(photoDto), Encoding.UTF8,
                                            "application/json")))
                                {
                                    if (response.StatusCode == HttpStatusCode.Created)
                                    {
                                        dynamic imageId = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                                        result.Add(imageId.imageId.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}