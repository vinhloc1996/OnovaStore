using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductImage
    {
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column("GeneralImageID")]
        public int GeneralImageId { get; set; }

        [ForeignKey("GeneralImageId")]
        [InverseProperty("ProductImage")]
        public GeneralImage GeneralImage { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("ProductImage")]
        public Product Product { get; set; }
    }
}
