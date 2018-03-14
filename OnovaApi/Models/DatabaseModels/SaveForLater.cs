using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class SaveForLater
    {
        [Column("CustomerID")]
        public string CustomerId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("SaveForLater")]
        public Customer Customer { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("SaveForLater")]
        public Product Product { get; set; }
    }
}
