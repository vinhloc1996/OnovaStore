using System;
using System.Collections.Generic;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerNotification = new HashSet<CustomerNotification>();
            CustomerRecentView = new HashSet<CustomerRecentView>();
            Review = new HashSet<Review>();
            SaveForLater = new HashSet<SaveForLater>();
            ShippingInfo = new HashSet<ShippingInfo>();
            UsefulReview = new HashSet<UsefulReview>();
            WishList = new HashSet<WishList>();
        }

        public string CustomerId { get; set; }
        public int? UserStatusId { get; set; }
        public DateTime JoinDate { get; set; }
        public string AnonymouseCustomerId { get; set; }

        public AnonymousCustomer AnonymouseCustomer { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public UserStatus UserStatus { get; set; }
        public CustomerCart CustomerCart { get; set; }
        public CustomerPurchaseInfo CustomerPurchaseInfo { get; set; }
        public ICollection<CustomerNotification> CustomerNotification { get; set; }
        public ICollection<CustomerRecentView> CustomerRecentView { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<SaveForLater> SaveForLater { get; set; }
        public ICollection<ShippingInfo> ShippingInfo { get; set; }
        public ICollection<UsefulReview> UsefulReview { get; set; }
        public ICollection<WishList> WishList { get; set; }
    }
}
