using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerRecentView
    {
        [Column("CustomerID")]
        public string CustomerId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerRecentView")]
        public Customer Customer { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("CustomerRecentView")]
        public Product Product { get; set; }
    }
}
