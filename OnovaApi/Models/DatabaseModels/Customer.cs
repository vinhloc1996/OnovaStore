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
            CustomerRecentView = new HashSet<CustomerRecentView>();
            Review = new HashSet<Review>();
            SaveForLater = new HashSet<SaveForLater>();
            ShippingInfo = new HashSet<ShippingInfo>();
            UsefulReview = new HashSet<UsefulReview>();
            WishList = new HashSet<WishList>();
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
        public CustomerPurchaseInfo CustomerPurchaseInfo { get; set; }
        [InverseProperty("Customer")]
        public ICollection<CustomerRecentView> CustomerRecentView { get; set; }
        [InverseProperty("Customer")]
        public ICollection<Review> Review { get; set; }
        [InverseProperty("Customer")]
        public ICollection<SaveForLater> SaveForLater { get; set; }
        [InverseProperty("Customer")]
        public ICollection<ShippingInfo> ShippingInfo { get; set; }
        [InverseProperty("Customer")]
        public ICollection<UsefulReview> UsefulReview { get; set; }
        [InverseProperty("Customer")]
        public ICollection<WishList> WishList { get; set; }
    }
}
