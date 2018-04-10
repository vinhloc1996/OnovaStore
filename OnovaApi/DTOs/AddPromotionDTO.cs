using System;

namespace OnovaApi.DTOs
{
    public class AddPromotionDTO
    {
        public string PromotionStatus { get; set; }
        public string PromotionName { get; set; }
        public string PromotionDescription { get; set; }
        public string PromotionCode { get; set; }
        public string TargetApply { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PercentOff { get; set; }
        public int? PromotionCategory { get; set; }
        public int? PromotionBrand { get; set; }
    }
}