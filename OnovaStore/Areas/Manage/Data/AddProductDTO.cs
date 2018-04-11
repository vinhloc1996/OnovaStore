using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace OnovaStore.Areas.Manage.Data
{
    public class AddProductDTO
    {
        public string Name { get; set; }
        public int ProductStatusId { get; set; }
        public string ProductLongDesc { get; set; }
        public string ProductShortDesc { get; set; }
        public int MaximumQuantity { get; set; }
        public double DisplayPrice { get; set; }
        public int Quantity { get; set; }
        public double RealPrice { get; set; }
        public double Weight { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string Slug { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;
        public string ThumbImageId { get; set; }
        public List<string> ProductImageIds { get; set; }
    }
}