using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class PromotionGroupProduct
    {
        public int PromotionId { get; set; }
        public int ProductId { get; set; }

        public Product Product { get; set; }
        public Promotion Promotion { get; set; }
    }
}
