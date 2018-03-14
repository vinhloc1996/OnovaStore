using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerPurchaseInfo
    {
        [Key]
        [Column("CustomerID")]
        public string CustomerId { get; set; }
        public int TotalSuccessOrder { get; set; }
        public double TotalMoneySpend { get; set; }
        public int TotalQuantityOfPurchasedProduct { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerPurchaseInfo")]
        public Customer Customer { get; set; }
    }
}
