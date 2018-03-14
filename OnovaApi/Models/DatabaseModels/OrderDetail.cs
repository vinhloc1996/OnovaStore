using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class OrderDetail
    {
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }
        public double? DisplayPrice { get; set; }
        public double? PriceDiscount { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        [Column("PromotionID")]
        public int? PromotionId { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("OrderDetail")]
        public Order Order { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("OrderDetail")]
        public Product Product { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("OrderDetail")]
        public Promotion Promotion { get; set; }
    }
}
