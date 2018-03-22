namespace OnovaStore.Models.Brand
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int? BrandImage { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Slug { get; set; }
    }
}