using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class PromotionCategory
    {
        public PromotionCategory()
        {
            ExcludeProductPromotionCategory = new HashSet<ExcludeProductPromotionCategory>();
        }

        public int PromotionId { get; set; }
        public int? CategoryId { get; set; }

        public Category Category { get; set; }
        public Promotion Promotion { get; set; }
        public ICollection<ExcludeProductPromotionCategory> ExcludeProductPromotionCategory { get; set; }
    }
}
