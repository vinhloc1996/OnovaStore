using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductSpecification
    {
        public ProductSpecification()
        {
            ProductSprcificationValue = new HashSet<ProductSprcificationValue>();
        }

        public int ProductSpecificationId { get; set; }
        public string ProductSpecificationName { get; set; }

        public ICollection<ProductSprcificationValue> ProductSprcificationValue { get; set; }
    }
}
