using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class UsefulReview
    {
        public int ReviewId { get; set; }
        public string CustomerId { get; set; }

        public Customer Customer { get; set; }
        public Review Review { get; set; }
    }
}
