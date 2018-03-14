using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerCart
    {
        public CustomerCart()
        {
            CustomerCartDetail = new HashSet<CustomerCartDetail>();
        }

        [Column("CustomerCartID")]
        public string CustomerCartId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Required]
        public byte[] LastUpdate { get; set; }
        public double? Tax { get; set; }
        public double? ShippingFee { get; set; }
        public double? DisplayPrice { get; set; }
        public double? PriceDiscount { get; set; }
        public double? TotalPrice { get; set; }
        public int? TotalQuantity { get; set; }
        [Column("PromotionID")]
        public int? PromotionId { get; set; }

        [ForeignKey("CustomerCartId")]
        [InverseProperty("CustomerCart")]
        public Customer CustomerCartNavigation { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("CustomerCart")]
        public Promotion Promotion { get; set; }
        [InverseProperty("CustomerCart")]
        public ICollection<CustomerCartDetail> CustomerCartDetail { get; set; }
    }
}
