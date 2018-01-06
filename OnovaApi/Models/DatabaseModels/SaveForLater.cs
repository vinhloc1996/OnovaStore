using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class SaveForLater
    {
        public string CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
