using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnovaStore.Areas.Manage.Models.Brand
{
    public class AddBrandViewModel
    {
        [Required]
        [DisplayName("Brand Name")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }
        [Required]
        [DisplayName("Contact Title")]
        public string ContactTitle { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Contact Phone")]
        public string ContactPhone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Contact Email")]
        public string ContactEmail { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        [DisplayName("Brand Image")]
        public IFormFile BrandImage { get; set; }
    }
}