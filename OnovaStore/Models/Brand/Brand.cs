using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OnovaStore.Models.Brand
{
    public class Brand
    {
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public int BrandId { get; set; }
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public string Name { get; set; }
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }
        [DisplayName("Contact Title")]
        public string ContactTitle { get; set; }
        [DisplayName("Contact Phone")]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }
        [Required]
        [DisplayName("Contact Email")]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public string BrandImage { get; set; }
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public string Slug { get; set; }
        [DisplayName("Is Hidden")]
        public bool? IsHide { get; set; }
    }
}