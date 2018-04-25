using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Customer
    {
        public Customer()
        {
            ShippingInfo = new HashSet<ShippingInfo>();
        }

        [Column("CustomerID")]
        public string CustomerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime JoinDate { get; set; }
        [Column("FacebookID")]
        public string FacebookId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Customer")]
        public ApplicationUser ApplicationUser { get; set; }
        [InverseProperty("CustomerCartNavigation")]
        public CustomerCart CustomerCart { get; set; }
        [InverseProperty("Customer")]
        public ICollection<ShippingInfo> ShippingInfo { get; set; }
    }
}
