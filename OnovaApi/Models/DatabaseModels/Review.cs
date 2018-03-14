using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Review
    {
        public Review()
        {
            InverseReplyReview = new HashSet<Review>();
            UsefulReview = new HashSet<UsefulReview>();
        }

        [Column("ReviewID")]
        public int ReviewId { get; set; }
        [Column("CustomerID")]
        [StringLength(450)]
        public string CustomerId { get; set; }
        [Column("ProductID")]
        public int? ProductId { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReleaseDate { get; set; }
        public byte Rating { get; set; }
        public bool? IsBought { get; set; }
        public int? UsefulCounting { get; set; }
        [Column("ReplyReviewID")]
        public int? ReplyReviewId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("Review")]
        public Customer Customer { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Review")]
        public Product Product { get; set; }
        [ForeignKey("ReplyReviewId")]
        [InverseProperty("InverseReplyReview")]
        public Review ReplyReview { get; set; }
        [InverseProperty("Review")]
        public ReviewConfirm ReviewConfirm { get; set; }
        [InverseProperty("ReplyReview")]
        public ICollection<Review> InverseReplyReview { get; set; }
        [InverseProperty("Review")]
        public ICollection<UsefulReview> UsefulReview { get; set; }
    }
}
