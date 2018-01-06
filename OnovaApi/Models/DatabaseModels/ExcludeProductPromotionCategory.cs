using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ExcludeProductPromotionCategory
    {
        public int PromotionId { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }

        public Category Category { get; set; }
        public Product Product { get; set; }
        public PromotionCategory Promotion { get; set; }
    }
}
