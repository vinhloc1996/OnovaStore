using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class UsefulReview
    {
        [Column("ReviewID")]
        public int ReviewId { get; set; }
        [Column("CustomerID")]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("UsefulReview")]
        public Customer Customer { get; set; }
        [ForeignKey("ReviewId")]
        [InverseProperty("UsefulReview")]
        public Review Review { get; set; }
    }
}
