using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ProductStatus
    {
        public ProductStatus()
        {
            Product = new HashSet<Product>();
        }

        public int ProductStatusId { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusDescription { get; set; }

        public ICollection<Product> Product { get; set; }
    }
}
