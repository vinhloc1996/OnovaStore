using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Option
    {
        public Option()
        {
            OptionDetail = new HashSet<OptionDetail>();
            ProductOptionGroup = new HashSet<ProductOptionGroup>();
        }

        public int OptionId { get; set; }
        public string OptionName { get; set; }

        public ICollection<OptionDetail> OptionDetail { get; set; }
        public ICollection<ProductOptionGroup> ProductOptionGroup { get; set; }
    }
}
