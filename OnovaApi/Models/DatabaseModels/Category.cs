using System;
using System.Collections.Generic;

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

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string CategoryCode { get; set; }
        public string Slug { get; set; }
        public int TotalProduct { get; set; }
        public int? CategoryImage { get; set; }
        public int? ParentCategoryId { get; set; }

        public GeneralImage CategoryImageNavigation { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<ExcludeProductPromotionCategory> ExcludeProductPromotionCategory { get; set; }
        public ICollection<Category> InverseParentCategory { get; set; }
        public ICollection<Product> Product { get; set; }
        public ICollection<PromotionCategory> PromotionCategory { get; set; }
    }
}
