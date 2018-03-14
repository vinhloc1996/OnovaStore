using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class WishList
    {
        [Column("CustomerID")]
        public string CustomerId { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("WishList")]
        public Customer Customer { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("WishList")]
        public Product Product { get; set; }
    }
}
