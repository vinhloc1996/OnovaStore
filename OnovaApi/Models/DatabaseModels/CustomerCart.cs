using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerCart
    {
        public CustomerCart()
        {
            CustomerCartDetail = new HashSet<CustomerCartDetail>();
        }

        public string CustomerId { get; set; }
        public int CustomerCartId { get; set; }
        public DateTime CreateDate { get; set; }
        public byte[] LastUpdate { get; set; }
        public double? Tax { get; set; }
        public double? DisplayPrice { get; set; }
        public double? PriceDiscount { get; set; }
        public double? TotalPrice { get; set; }
        public int? TotalQuantity { get; set; }
        public int? PromotionId { get; set; }

        public Customer Customer { get; set; }
        public Promotion Promotion { get; set; }
        public ICollection<CustomerCartDetail> CustomerCartDetail { get; set; }
    }
}
