using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ExcludeProductPromotionBrand
    {
        public int PromotionId { get; set; }
        public int BrandId { get; set; }
        public int ProductId { get; set; }

        public Brand Brand { get; set; }
        public Product Product { get; set; }
        public PromotionBrand Promotion { get; set; }
    }
}
