using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class PromotionGroupProduct
    {
        [Column("PromotionID")]
        public int PromotionId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("PromotionGroupProduct")]
        public Product Product { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("PromotionGroupProduct")]
        public Promotion Promotion { get; set; }
    }
}
