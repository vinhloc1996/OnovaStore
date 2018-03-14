using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductPriceOff
    {
        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal PercentOff { get; set; }
        public double PriceOff { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductPriceOff")]
        public Product Product { get; set; }
    }
}
