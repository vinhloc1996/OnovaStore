using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnovaStore.Areas.Manage.Models.Product
{
    public class AddProductViewModel
    {
        [Required]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Product Status")]
        public int ProductStatusId { get; set; }
        [Required]
        [DisplayName("Product Long Description")]
        public string ProductLongDesc { get; set; }
        [Required]
        [DisplayName("Product Short Description")]
        public string ProductShortDesc { get; set; }
        [Required]
        [DisplayName("Maximum Quantity In A Order")]
        public int MaximumQuantity { get; set; }
        [Required]
        [DisplayName("Product Saling Price")]
        public double DisplayPrice { get; set; }
        [Required]
        [DisplayName("Product Quantity")]
        public int Quantity { get; set; }
        [Required]
        [DisplayName("Product Base Price")]
        public double RealPrice { get; set; }
        [DisplayName("Product Weight")]
        public double Weight { get; set; }
        [Required]
        [DisplayName("Brand Name")]
        public int BrandId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Product Thumbnail Image")]
        [DataType(DataType.Upload)]
        public IFormFile ThumbImage { get; set; }
        [Required]
        [DisplayName("Product Images")]
        [DataType(DataType.Upload)]
        public List<IFormFile> ProductImages { get; set; }
    }
}