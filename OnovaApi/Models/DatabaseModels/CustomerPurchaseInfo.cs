using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerPurchaseInfo
    {
        public string CustomerId { get; set; }
        public int TotalSuccessOrder { get; set; }
        public double TotalMoneySpend { get; set; }
        public int TotalQuantityOfPurchasedProduct { get; set; }

        public Customer Customer { get; set; }
    }
}
