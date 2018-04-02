using System;

namespace OnovaStore.Areas.Manage.Data
{
    public class ImageUploadDTO
    {
        public DateTime AddDate { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public string Name { get; set; }
        public string StaffId { get; set; }
    }
}