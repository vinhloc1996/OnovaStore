using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductSpecification
    {
        public ProductSpecification()
        {
            ProductSprcificationValue = new HashSet<ProductSprcificationValue>();
        }

        [Column("ProductSpecificationID")]
        public int ProductSpecificationId { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductSpecificationName { get; set; }

        [InverseProperty("ProductSpecification")]
        public ICollection<ProductSprcificationValue> ProductSprcificationValue { get; set; }
    }
}
