﻿using System;

namespace OnovaStore.Areas.Manage.Data
{
    public class ImageUploadDTO
    {
        public ImageUploadDTO()
        {
            AddDate = DateTime.Now;
        }

        public DateTime AddDate { get; set; }
        public string ImageUrl { get; set; }
        public string StaffId { get; set; }
        public string ImageId { get; set; }
    }
}