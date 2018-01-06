using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductOptionGroup
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }
        public int OptionDetailId { get; set; }

        public Option Option { get; set; }
        public OptionDetail OptionDetail { get; set; }
        public Product Product { get; set; }
    }
}
