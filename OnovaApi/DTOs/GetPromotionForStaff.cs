namespace OnovaApi.DTOs
{
    public class GetPromotionForStaff
    {
        public int PromotionId { get; set; }
        public string PromotionCode { get; set; }
        public string TargetApply { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
    }
}