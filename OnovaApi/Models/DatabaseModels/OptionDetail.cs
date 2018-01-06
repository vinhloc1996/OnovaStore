using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class OptionDetail
    {
        public OptionDetail()
        {
            ProductOptionGroup = new HashSet<ProductOptionGroup>();
        }

        public int OptionDetailId { get; set; }
        public string OptionDetailName { get; set; }
        public int? OptionId { get; set; }

        public Option Option { get; set; }
        public ICollection<ProductOptionGroup> ProductOptionGroup { get; set; }
    }
}
