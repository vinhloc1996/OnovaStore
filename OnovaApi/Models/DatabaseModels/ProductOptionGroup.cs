using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductOptionGroup
    {
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column("OptionID")]
        public int OptionId { get; set; }
        [Column("OptionDetailID")]
        public int OptionDetailId { get; set; }

        [ForeignKey("OptionId")]
        [InverseProperty("ProductOptionGroup")]
        public Option Option { get; set; }
        [ForeignKey("OptionDetailId")]
        [InverseProperty("ProductOptionGroup")]
        public OptionDetail OptionDetail { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("ProductOptionGroup")]
        public Product Product { get; set; }
    }
}
