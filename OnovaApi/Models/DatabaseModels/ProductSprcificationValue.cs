using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductSprcificationValue
    {
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column("ProductSpecificationID")]
        public int ProductSpecificationId { get; set; }
        [Required]
        [StringLength(255)]
        public string ProductSpecificationValue { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("ProductSprcificationValue")]
        public Product Product { get; set; }
        [ForeignKey("ProductSpecificationId")]
        [InverseProperty("ProductSprcificationValue")]
        public ProductSpecification ProductSpecification { get; set; }
    }
}
