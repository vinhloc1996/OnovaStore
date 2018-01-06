using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class WishList
    {
        public string CustomerId { get; set; }
        public int ProductId { get; set; }

        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
