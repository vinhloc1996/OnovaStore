using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class GeneralImage
    {
        public GeneralImage()
        {
            Brand = new HashSet<Brand>();
            Category = new HashSet<Category>();
            Product = new HashSet<Product>();
            ProductImage = new HashSet<ProductImage>();
            Promotion = new HashSet<Promotion>();
        }

        [Key]
        [Column("GeneralImageID")]
        public string GeneralImageId { get; set; }
        [Required]
        [Column("ImageURL", TypeName = "text")]
        public string ImageUrl { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AddDate { get; set; }
        [Column("StaffID")]
        [StringLength(450)]
        public string StaffId { get; set; }

        [ForeignKey("StaffId")]
        [InverseProperty("GeneralImage")]
        public Staff Staff { get; set; }
        [InverseProperty("BrandImageNavigation")]
        public ICollection<Brand> Brand { get; set; }
        [InverseProperty("CategoryImageNavigation")]
        public ICollection<Category> Category { get; set; }
        [InverseProperty("ProductThumbImageNavigation")]
        public ICollection<Product> Product { get; set; }
        [InverseProperty("GeneralImage")]
        public ICollection<ProductImage> ProductImage { get; set; }
        [InverseProperty("PromotionImageNavigation")]
        public ICollection<Promotion> Promotion { get; set; }
    }
}
