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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnovaStore.Areas.Manage.Data;
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
                var staffId = _claimPrincipalManager.Id;
                var uploadResult = new ImageUploadResult();
                var length = model.Files.Count;
                var size = model.Files.Sum(e => e.Length);

                if (size > 5242880)
                {
                    ModelState.AddModelError(string.Empty, "File Size is not greater than 5MB");
                    return View(model);
                }

                if (length > 0)
                {
                    foreach (var file in model.Files)
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
                                            return View(); //should be redirect to list of photo(product,brand,category,promotion)
                                        }
                                        
                                        ModelState.AddModelError(string.Empty, "Cannot add image to database");
                                    }
                                }
                            }

                            ModelState.AddModelError(string.Empty, "Cannot upload image to server");
                        }
                    }
                }
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
    }
}