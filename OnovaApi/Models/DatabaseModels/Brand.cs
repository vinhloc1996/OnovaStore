using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Brand
    {
        public Brand()
        {
            ExcludeProductPromotionBrand = new HashSet<ExcludeProductPromotionBrand>();
            Product = new HashSet<Product>();
            PromotionBrand = new HashSet<PromotionBrand>();
        }

        [Column("BrandID")]
        public int BrandId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(255)]
        public string ContactName { get; set; }
        [StringLength(20)]
        public string ContactTitle { get; set; }
        [StringLength(20)]
        public string ContactPhone { get; set; }
        [Required]
        [StringLength(254)]
        public string ContactEmail { get; set; }
        public string BrandImage { get; set; }
        [Required]
        [StringLength(200)]
        public string Slug { get; set; }

        [ForeignKey("BrandImage")]
        [InverseProperty("Brand")]
        public GeneralImage BrandImageNavigation { get; set; }
        [InverseProperty("Brand")]
        public ICollection<ExcludeProductPromotionBrand> ExcludeProductPromotionBrand { get; set; }
        [InverseProperty("Brand")]
        public ICollection<Product> Product { get; set; }
        [InverseProperty("Brand")]
        public ICollection<PromotionBrand> PromotionBrand { get; set; }
    }
}
