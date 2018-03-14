using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class AnonymousCustomerCartDetail
    {
        [Column("AnonymousCustomerCartID")]
        public string AnonymousCustomerCartId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }
        public double? DisplayPrice { get; set; }
        public double? PriceDiscount { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        [Column("PromotionID")]
        public int? PromotionId { get; set; }

        [ForeignKey("AnonymousCustomerCartId")]
        [InverseProperty("AnonymousCustomerCartDetail")]
        public AnonymousCustomerCart AnonymousCustomerCart { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("AnonymousCustomerCartDetail")]
        public Product Product { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("AnonymousCustomerCartDetail")]
        public Promotion Promotion { get; set; }
    }
}
