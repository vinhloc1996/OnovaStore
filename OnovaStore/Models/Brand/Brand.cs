namespace OnovaStore.Models.Brand
{
    public class Brand
    {
        public Brand()
        {
        }
        
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int TotalProduct { get; set; }
        public string BrandImage { get; set; }
        public string Slug { get; set; }
        
    }
}