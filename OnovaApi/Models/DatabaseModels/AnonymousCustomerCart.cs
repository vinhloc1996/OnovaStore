using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class AnonymousCustomerCart
    {
        public AnonymousCustomerCart()
        {
            AnonymousCustomerCartDetail = new HashSet<AnonymousCustomerCartDetail>();
        }

        [Column("AnonymousCustomerCartID")]
        public string AnonymousCustomerCartId { get; set; }
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

        [ForeignKey("AnonymousCustomerCartId")]
        [InverseProperty("AnonymousCustomerCart")]
        public AnonymousCustomer AnonymousCustomerCartNavigation { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("AnonymousCustomerCart")]
        public Promotion Promotion { get; set; }
        [InverseProperty("AnonymousCustomerCart")]
        public ICollection<AnonymousCustomerCartDetail> AnonymousCustomerCartDetail { get; set; }
    }
}
