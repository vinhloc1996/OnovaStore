using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class GeneralImage
    {
        public GeneralImage()
        {
            Brand = new HashSet<Brand>();
            Category = new HashSet<Category>();
            Product = new HashSet<Product>();
            ProductImage = new HashSet<ProductImage>();
            Promotion = new HashSet<Promotion>();
        }

        public int GeneralImageId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public DateTime AddDate { get; set; }
        public string StaffId { get; set; }

        public Staff Staff { get; set; }
        public ICollection<Brand> Brand { get; set; }
        public ICollection<Category> Category { get; set; }
        public ICollection<Product> Product { get; set; }
        public ICollection<ProductImage> ProductImage { get; set; }
        public ICollection<Promotion> Promotion { get; set; }
    }
}
