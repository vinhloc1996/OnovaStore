using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductStatus
    {
        public ProductStatus()
        {
            Product = new HashSet<Product>();
        }

        [Column("ProductStatusID")]
        public int ProductStatusId { get; set; }
        [StringLength(50)]
        public string StatusCode { get; set; }
        [StringLength(100)]
        public string StatusName { get; set; }
        [StringLength(500)]
        public string StatusDescription { get; set; }

        [InverseProperty("ProductStatus")]
        public ICollection<Product> Product { get; set; }
    }
}
