using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnovaStore.Areas.Manage.Models.Image
{
    public class UploadImageViewModel
    {
        [Required]
        [DataType(DataType.Upload)]
        public List<IFormFile> Files { get; set; }
    }
}