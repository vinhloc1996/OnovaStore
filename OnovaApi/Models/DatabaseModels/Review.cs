using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Review
    {
        public Review()
        {
            InverseReplyReview = new HashSet<Review>();
            UsefulReview = new HashSet<UsefulReview>();
        }

        public int ReviewId { get; set; }
        public string CustomerId { get; set; }
        public int? ProductId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime ReleaseDate { get; set; }
        public byte Rating { get; set; }
        public bool IsBought { get; set; }
        public int? UsefulCounting { get; set; }
        public int? ReplyReviewId { get; set; }

        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public Review ReplyReview { get; set; }
        public ReviewConfirm ReviewConfirm { get; set; }
        public ICollection<Review> InverseReplyReview { get; set; }
        public ICollection<UsefulReview> UsefulReview { get; set; }
    }
}
