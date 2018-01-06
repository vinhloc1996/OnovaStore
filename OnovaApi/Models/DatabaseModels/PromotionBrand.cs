using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class PromotionBrand
    {
        public PromotionBrand()
        {
            ExcludeProductPromotionBrand = new HashSet<ExcludeProductPromotionBrand>();
        }

        public int PromotionId { get; set; }
        public int? BrandId { get; set; }

        public Brand Brand { get; set; }
        public Promotion Promotion { get; set; }
        public ICollection<ExcludeProductPromotionBrand> ExcludeProductPromotionBrand { get; set; }
    }
}
