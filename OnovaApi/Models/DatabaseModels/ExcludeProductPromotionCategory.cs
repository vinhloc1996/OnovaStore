using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ExcludeProductPromotionCategory
    {
        [Column("PromotionID")]
        public int PromotionId { get; set; }
        [Column("CategoryID")]
        public int CategoryId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("ExcludeProductPromotionCategory")]
        public Category Category { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("ExcludeProductPromotionCategory")]
        public Product Product { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("ExcludeProductPromotionCategory")]
        public PromotionCategory Promotion { get; set; }
    }
}
