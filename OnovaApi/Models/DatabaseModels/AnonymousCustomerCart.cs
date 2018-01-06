using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class AnonymousCustomerCart
    {
        public AnonymousCustomerCart()
        {
            AnonymousCustomerCartDetail = new HashSet<AnonymousCustomerCartDetail>();
        }

        public string AnonymousCustomerId { get; set; }
        public int AnonymousCustomerCartId { get; set; }
        public DateTime CreateDate { get; set; }
        public byte[] LastUpdate { get; set; }
        public double? Tax { get; set; }
        public double? DisplayPrice { get; set; }
        public double? PriceDiscount { get; set; }
        public double? TotalPrice { get; set; }
        public int? TotalQuantity { get; set; }
        public int? PromotionId { get; set; }

        public AnonymousCustomer AnonymousCustomer { get; set; }
        public Promotion Promotion { get; set; }
        public ICollection<AnonymousCustomerCartDetail> AnonymousCustomerCartDetail { get; set; }
    }
}
