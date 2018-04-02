using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnovaStore.Areas.Manage.Data;
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
        private Cloudinary _cloudinary;

        public ManagerController(IClaimPrincipalManager claimPrincipalManager, IOptions<CloudinarySettings> cloudinarySettings)
        {
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
        public IActionResult UploadImage([FromForm] UploadImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var staffId = _claimPrincipalManager.Id;
                var image = model.ImageFile;
                var uploadResult = new ImageUploadResult();

                if (image.Length > 0)
                {
                    using (var stream = image.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(model.Name, stream)
                        };

                        uploadResult = _cloudinary.Upload(uploadParams);
                    }

                    if (uploadResult.StatusCode == HttpStatusCode.OK)
                    {
                        var photoDto = new ImageUploadDTO
                        {
                            AddDate = uploadResult.CreatedAt,
                            Name = model.Name,
                            StaffId = staffId,
                            ImageUrl = uploadResult.Uri.ToString(),
                            PublicId = uploadResult.PublicId
                        };
                    }
                }
            }

            return View(model);
        }
    }
}