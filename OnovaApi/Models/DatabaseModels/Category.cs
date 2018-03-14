using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Category
    {
        public Category()
        {
            ExcludeProductPromotionCategory = new HashSet<ExcludeProductPromotionCategory>();
            InverseParentCategory = new HashSet<Category>();
            Product = new HashSet<Product>();
            PromotionCategory = new HashSet<PromotionCategory>();
        }

        [Column("CategoryID")]
        public int CategoryId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string CategoryCode { get; set; }
        [Required]
        [StringLength(200)]
        public string Slug { get; set; }
        public int TotalProduct { get; set; }
        public int? CategoryImage { get; set; }
        [Column("ParentCategoryID")]
        public int? ParentCategoryId { get; set; }

        [ForeignKey("CategoryImage")]
        [InverseProperty("Category")]
        public GeneralImage CategoryImageNavigation { get; set; }
        [ForeignKey("ParentCategoryId")]
        [InverseProperty("InverseParentCategory")]
        public Category ParentCategory { get; set; }
        [InverseProperty("Category")]
        public ICollection<ExcludeProductPromotionCategory> ExcludeProductPromotionCategory { get; set; }
        [InverseProperty("ParentCategory")]
        public ICollection<Category> InverseParentCategory { get; set; }
        [InverseProperty("Category")]
        public ICollection<Product> Product { get; set; }
        [InverseProperty("Category")]
        public ICollection<PromotionCategory> PromotionCategory { get; set; }
    }
}
