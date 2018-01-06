using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerCartDetail
    {
        public int CustomerCartId { get; set; }
        public int ProductId { get; set; }
        public double? DisplayPrice { get; set; }
        public double? PriceDiscount { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? PromotionId { get; set; }

        public CustomerCart CustomerCart { get; set; }
        public Product Product { get; set; }
        public Promotion Promotion { get; set; }
    }
}
