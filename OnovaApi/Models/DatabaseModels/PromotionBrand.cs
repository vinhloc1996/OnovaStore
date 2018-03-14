using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class PromotionBrand
    {
        public PromotionBrand()
        {
            ExcludeProductPromotionBrand = new HashSet<ExcludeProductPromotionBrand>();
        }

        [Key]
        [Column("PromotionID")]
        public int PromotionId { get; set; }
        [Column("BrandID")]
        public int? BrandId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("PromotionBrand")]
        public Brand Brand { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("PromotionBrand")]
        public Promotion Promotion { get; set; }
        [InverseProperty("Promotion")]
        public ICollection<ExcludeProductPromotionBrand> ExcludeProductPromotionBrand { get; set; }
    }
}
