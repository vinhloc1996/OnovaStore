using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductSprcificationValue
    {
        public int ProductId { get; set; }
        public int ProductSpecificationId { get; set; }
        public string ProductSpecificationValue { get; set; }

        public Product Product { get; set; }
        public ProductSpecification ProductSpecification { get; set; }
    }
}
