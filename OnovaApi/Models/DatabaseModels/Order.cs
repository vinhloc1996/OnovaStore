using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Order
    {
        public Order()
        {
            OrderDetail = new HashSet<OrderDetail>();
        }

        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("OrderStatusID")]
        public int? OrderStatusId { get; set; }
        [StringLength(20)]
        public string OrderTrackingNumber { get; set; }
        [Column("CartID")]
        [StringLength(450)]
        public string CartId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }
        public double? Tax { get; set; }
        public double? ShippingFee { get; set; }
        public double? DisplayPrice { get; set; }
        public double? PriceDiscount { get; set; }
        public double? TotalPrice { get; set; }
        public int? TotalQuantity { get; set; }
        [Column("PromotionID")]
        public int? PromotionId { get; set; }
        [StringLength(256)]
        public string FullName { get; set; }
        [Required]
        [StringLength(255)]
        public string AddressLine1 { get; set; }
//        [StringLength(255)]
//        public string AddressLine2 { get; set; }
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        [Required]
        [StringLength(50)]
        public string Zip { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
        [Column(TypeName = "date")]
        public DateTime EstimateShippingDate { get; set; }
        [Required]
        [StringLength(250)]
        public string PaymentTokenId { get; set; }
        [Required]
        [StringLength(100)]
        public string PaymentStatus { get; set; }
        [Required]
        [StringLength(250)]
        public string InvoiceTokenId { get; set; }

        [ForeignKey("OrderStatusId")]
        [InverseProperty("Order")]
        public OrderStatus OrderStatus { get; set; }
        [ForeignKey("PromotionId")]
        [InverseProperty("Order")]
        public Promotion Promotion { get; set; }
        [InverseProperty("Order")]
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
