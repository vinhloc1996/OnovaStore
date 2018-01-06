using System;
using System.Collections.Generic;

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
            PromotionGroupProduct = new HashSet<PromotionGroupProduct>();
        }

        public int PromotionId { get; set; }
        public string PromotionStatus { get; set; }
        public string PromotionName { get; set; }
        public string PromotionDescription { get; set; }
        public string PromotionCode { get; set; }
        public int? PromotionImage { get; set; }
        public string TargetApply { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal PercentOff { get; set; }
        public byte[] LastUpdateDate { get; set; }

        public GeneralImage PromotionImageNavigation { get; set; }
        public PromotionBrand PromotionBrand { get; set; }
        public PromotionCategory PromotionCategory { get; set; }
        public ICollection<AnonymousCustomerCart> AnonymousCustomerCart { get; set; }
        public ICollection<AnonymousCustomerCartDetail> AnonymousCustomerCartDetail { get; set; }
        public ICollection<CustomerCart> CustomerCart { get; set; }
        public ICollection<CustomerCartDetail> CustomerCartDetail { get; set; }
        public ICollection<PromotionGroupProduct> PromotionGroupProduct { get; set; }
    }
}
