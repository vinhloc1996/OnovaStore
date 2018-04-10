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
using OnovaStore.Areas.Manage.Models.Category;
using OnovaStore.Areas.Manage.Models.Image;
using OnovaStore.Helpers;

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
        public async Task<IActionResult> Brands(string sortOrder, string currentFilter, string searchString, int? page = 1)
        {
            var brands = new List<GetBrandForStaff>();

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
                "id",
                "id_desc",
                "name",
                "name_desc",
                "totalproduct",
                "totalproduct_desc"
            };

            sortOrder = string.IsNullOrEmpty(sortOrder) || !sortQuery.Contains(sortOrder)
                ? "id"
                : sortOrder.Trim().ToLower();

            var queryString = nameof(sortOrder) + "=" + sortOrder + (!string.IsNullOrEmpty(searchString)
                                  ? "&" + nameof(searchString) + "=" + searchString
                                  : "");

            ViewData["SortOrder"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.GetAsync("/api/brand/GetBrandsForStaff?" + queryString))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        brands = JsonConvert.DeserializeObject<List<GetBrandForStaff>>(
                            await response.Content.ReadAsStringAsync());

                        
                    }
                }
            }

            int pageSize = 5;
            ViewData["LengthEntry"] = brands.Count;
            ViewData["CurrentEntry"] = pageSize * page;
            
            return View(PaginatedList<GetBrandForStaff>.CreateAsync(brands, page ?? 1, pageSize));
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
                            return RedirectToAction("Brands");
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditBrand([FromRoute] int id)
        {
            var brand = new EditBrandViewModel();

            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.GetAsync("/api/brand/" + id))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        brand = JsonConvert.DeserializeObject<EditBrandViewModel>(
                            await response.Content.ReadAsStringAsync());
                    }
                    else
                    {
                        return RedirectToAction("Brands");
                    }
                }
            }

            return View(brand);
        }

        [HttpPost]
        public async Task<IActionResult> EditBrand([FromRoute] int id, EditBrandViewModel editBrandViewModel)
        {
            if (ModelState.IsValid)
            {
                if (id == editBrandViewModel.BrandId)
                {
                    using (var client = _restClient.CreateClient(User))
                    {
                        using (
                            var response = await client.PutAsync("/api/brand",
                                new StringContent(JsonConvert.SerializeObject(editBrandViewModel), Encoding.UTF8,
                                    "application/json")))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                return RedirectToAction("Brands");
                            }
                        }
                    }
                }

                ModelState.AddModelError(String.Empty, "Update failed");
            }


            return View(editBrandViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Categories(string sortOrder, string currentFilter, string searchString, int? page = 1)
        {
            var categories = new List<GetCategoryForStaff>();

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
                "id",
                "id_desc",
                "name",
                "name_desc",
                "totalproduct",
                "totalproduct_desc"
            };

            sortOrder = string.IsNullOrEmpty(sortOrder) || !sortQuery.Contains(sortOrder)
                ? "id"
                : sortOrder.Trim().ToLower();

            var queryString = nameof(sortOrder) + "=" + sortOrder + (!string.IsNullOrEmpty(searchString)
                                  ? "&" + nameof(searchString) + "=" + searchString
                                  : "");

            ViewData["SortOrder"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.GetAsync("/api/category/GetCategoriesForStaff?" + queryString))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        categories = JsonConvert.DeserializeObject<List<GetCategoryForStaff>>(
                            await response.Content.ReadAsStringAsync());
                    }
                }
            }

            int pageSize = 5;
            ViewData["LengthEntry"] = categories.Count;
            ViewData["CurrentEntry"] = pageSize * page;

            return View(PaginatedList<GetCategoryForStaff>.CreateAsync(categories, page ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> AddCategory()
        {
            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.GetAsync("/api/category/GetCategories"))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        ViewData["ListCategories"] = JsonConvert.DeserializeObject<List<GetCategoriesDTO>>(
                            await response.Content.ReadAsStringAsync());
                    }
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var size = model.CategoryImage.Length;

                if (size > 5242880)
                {
                    ModelState.AddModelError("OverLength", "File Size is not greater than 5MB");
                    return View(model);
                }

                var imageId = await UploadImages(new List<IFormFile>
                {
                    model.CategoryImage
                });

                //check parent categori is existed
                if (model.ParentCategoryID != 0)
                {
                    using (var client = _restClient.CreateClient(User))
                    {
                        using (
                            var response =
                                await client.GetAsync("/api/category/CheckCategoryExisted?id=" +
                                                      model.ParentCategoryID))
                        {
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                ModelState.AddModelError(string.Empty, "Category Parent Id is not existed");
                                return View(model);
                            }
                        }
                    }
                }
                else
                {
                    model.ParentCategoryID = null;
                }
                

                var category = new AddCategoryDTO()
                {
                    CategoryImage = imageId[0],
                    Slug = model.Name.URLFriendly(),
                    Name = model.Name,
                    ParentCategoryID = model.ParentCategoryID
                };

                using (var client = _restClient.CreateClient(User))
                {
                    using (
                        var response = await client.PostAsync("/api/category",
                            new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8,
                                "application/json")))
                    {
                        if (response.StatusCode == HttpStatusCode.Created)
                        {
                            return RedirectToAction("Categories");
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory([FromRoute] int id)
        {
            var category = new EditCategoryViewModel();

            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.GetAsync("/api/category/GetCategories"))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var list = JsonConvert.DeserializeObject<List<GetCategoriesDTO>>(
                            await response.Content.ReadAsStringAsync());

                        if (list.Count > 0)
                        {
                            var self = list.Single(c => c.CategoryId == id);
                            list.Remove(self);
                        }

                        ViewData["ListCategories"] = list;
                    }
                }
            }

            using (var client = _restClient.CreateClient(User))
            {
                using (
                    var response = await client.GetAsync("/api/category/" + id))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        category = JsonConvert.DeserializeObject<EditCategoryViewModel>(
                            await response.Content.ReadAsStringAsync());

                        category.IsHide = category.IsHide == true;
                    }
                    else
                    {
                        return RedirectToAction("Categories");
                    }
                }
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory([FromRoute] int id, EditCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                if (id == category.CategoryId)
                {
                    using (var client = _restClient.CreateClient(User))
                    {
                        using (
                            var response = await client.PutAsync("/api/category",
                                new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8,
                                    "application/json")))
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                return RedirectToAction("Categories");
                            }
                        }
                    }
                }

                ModelState.AddModelError(String.Empty, "Update failed");
            }


            return View(category);
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