using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace OnovaStore.Areas.Manage.Models.Category
{
    public class EditCategoryViewModel
    {
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Parent Category")]
        public int? ParentCategoryID { get; set; }
        [HiddenInput]
        [ReadOnly(true)]
        public string CategoryImage { get; set; }
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public string Slug { get; set; }
        [Required]
        [HiddenInput]
        [ReadOnly(true)]
        public int TotalProduct { get; set; }
        [DisplayName("Is Hidden")]
        public bool? IsHide { get; set; }
    }
}