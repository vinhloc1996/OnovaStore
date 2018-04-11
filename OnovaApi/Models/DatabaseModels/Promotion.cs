using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Promotion
    {
        public Promotion()
        {
            AnonymousCustomerCart = new HashSet<AnonymousCustomerCart>();
            AnonymousCustomerCartDetail = new HashSet<AnonymousCustomerCartDetail>();
            CustomerCart = new HashSet<CustomerCart>();
            CustomerCartDetail = new HashSet<CustomerCartDetail>();
            Order = new HashSet<Order>();
            OrderDetail = new HashSet<OrderDetail>();
        }

        [Column("PromotionID")]
        public int PromotionId { get; set; }
        [Required]
        [StringLength(50)]
        public string PromotionStatus { get; set; }
        [Required]
        [StringLength(255)]
        public string PromotionName { get; set; }
        [Column(TypeName = "ntext")]
        public string PromotionDescription { get; set; }
        [Required]
        [StringLength(50)]
        public string PromotionCode { get; set; }
        public string PromotionImage { get; set; }
        [Required]
        [StringLength(50)]
        public string TargetApply { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal PercentOff { get; set; }
        [Required]
        public byte[] LastUpdateDate { get; set; }

        [ForeignKey("PromotionImage")]
        [InverseProperty("Promotion")]
        public GeneralImage PromotionImageNavigation { get; set; }
        [InverseProperty("Promotion")]
        public PromotionBrand PromotionBrand { get; set; }
        [InverseProperty("Promotion")]
        public PromotionCategory PromotionCategory { get; set; }
        [InverseProperty("Promotion")]
        public ICollection<AnonymousCustomerCart> AnonymousCustomerCart { get; set; }
        [InverseProperty("Promotion")]
        public ICollection<AnonymousCustomerCartDetail> AnonymousCustomerCartDetail { get; set; }
        [InverseProperty("Promotion")]
        public ICollection<CustomerCart> CustomerCart { get; set; }
        [InverseProperty("Promotion")]
        public ICollection<CustomerCartDetail> CustomerCartDetail { get; set; }
        [InverseProperty("Promotion")]
        public ICollection<Order> Order { get; set; }
        [InverseProperty("Promotion")]
        public ICollection<OrderDetail> OrderDetail { get; set; }
    }
}
