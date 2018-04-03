using System;

namespace OnovaApi.DTOs
{
    public class ImageUploadDTO
    {
        public DateTime AddDate { get; set; }
        public string ImageUrl { get; set; }
        public string StaffId { get; set; }
        public string ImageId { get; set; }
    }
}