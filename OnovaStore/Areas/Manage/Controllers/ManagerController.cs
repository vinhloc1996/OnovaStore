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
        public IActionResult UploadImage(UploadImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var staffId = _claimPrincipalManager.Id;
                var uploadResult = new ImageUploadResult();
                var length = model.Files.Count;

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
                                    AddDate = uploadResult.CreatedAt,
                                    StaffId = staffId,
                                    ImageUrl = uploadResult.Uri.ToString(),
                                    PublicId = uploadResult.PublicId
                                };
                            }
                        }
                    }
                }
            }

            return View(model);
        }
    }
}