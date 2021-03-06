﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerCartDetail
    {
        [Column("CustomerCartID")]
        public string CustomerCartId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }
        public double? DisplayPrice { get; set; }
        public double? PriceDiscount { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        [Column("PromotionID")]
        public int? PromotionId { get; set; }

        [ForeignKey("CustomerCartId")]
        [InverseProperty("CustomerCartDetail")]
        public CustomerCart CustomerCart { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("CustomerCartDetail")]
        public Product Product { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("CustomerCartDetail")]
        public Promotion Promotion { get; set; }
    }
}
