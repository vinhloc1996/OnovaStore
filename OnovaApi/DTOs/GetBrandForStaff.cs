namespace OnovaApi.DTOs
{
    public class GetBrandForStaff
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string ContactEmail { get; set; }
        public double Rate { get; set; }
        public double TotalSales { get; set; }
        public int TotalProducts { get; set; }
    }
}