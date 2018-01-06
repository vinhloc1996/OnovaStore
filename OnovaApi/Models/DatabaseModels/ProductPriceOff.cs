using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductPriceOff
    {
        public int ProductId { get; set; }
        public decimal PercentOff { get; set; }
        public double PriceOff { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Product Product { get; set; }
    }
}
