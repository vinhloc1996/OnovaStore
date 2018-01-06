using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Brand
    {
        public Brand()
        {
            ExcludeProductPromotionBrand = new HashSet<ExcludeProductPromotionBrand>();
            Product = new HashSet<Product>();
            PromotionBrand = new HashSet<PromotionBrand>();
        }

        public int BrandId { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int? BrandImage { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Slug { get; set; }

        public GeneralImage BrandImageNavigation { get; set; }
        public ICollection<ExcludeProductPromotionBrand> ExcludeProductPromotionBrand { get; set; }
        public ICollection<Product> Product { get; set; }
        public ICollection<PromotionBrand> PromotionBrand { get; set; }
    }
}
