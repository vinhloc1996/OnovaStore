using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class PromotionCategory
    {
        public PromotionCategory()
        {
        }

        [Key]
        [Column("PromotionID")]
        public int PromotionId { get; set; }
        [Column("CategoryID")]
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("PromotionCategory")]
        public Category Category { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("PromotionCategory")]
        public Promotion Promotion { get; set; }
    }
}
