using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnovaStore.Areas.Manage.Models.Category
{
    public class AddCategoryViewModel
    {
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Parent Category")]
        public int? ParentCategoryID { get; set; }
        [Required]
        [DataType(DataType.Upload)]
        [DisplayName("Category Image")]
        public IFormFile CategoryImage { get; set; }
    }
}