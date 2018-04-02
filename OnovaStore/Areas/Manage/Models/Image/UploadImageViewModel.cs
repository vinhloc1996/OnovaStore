using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnovaStore.Areas.Manage.Models.Image
{
    public class UploadImageViewModel
    {
        public UploadImageViewModel()
        {
            AddDate = DateTime.Now;
        }
        
        public DateTime AddDate { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
        [Required]
        public string Name { get; set; }
        public string StaffId { get; set; }

        
    }
}