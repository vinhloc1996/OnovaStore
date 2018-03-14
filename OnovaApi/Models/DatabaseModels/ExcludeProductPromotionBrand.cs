using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ExcludeProductPromotionBrand
    {
        [Column("PromotionID")]
        public int PromotionId { get; set; }
        [Column("BrandID")]
        public int BrandId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("ExcludeProductPromotionBrand")]
        public Brand Brand { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("ExcludeProductPromotionBrand")]
        public Product Product { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("ExcludeProductPromotionBrand")]
        public PromotionBrand Promotion { get; set; }
    }
}
