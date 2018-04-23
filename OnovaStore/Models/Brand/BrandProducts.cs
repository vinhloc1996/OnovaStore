using System.Collections.Generic;

namespace OnovaStore.Models.Brand
{
    public class BrandProducts
    {
        public int productId { get; set; }
        public double displayPrice { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public object productThumbImage { get; set; }
        public string categoryName { get; set; }
        public string statusCode { get; set; }
        public string categorySlug { get; set; }
    }

    public class BrandInfo
    {
        public int brandId { get; set; }
        public object brandImage { get; set; }
        public string name { get; set; }
        public int totalProduct { get; set; }
        public string slug { get; set; }
        public List<BrandProducts> products { get; set; }
    }

    public class RootBrandProducts
    {
        public string status { get; set; }
        public string message { get; set; }
        public BrandInfo brands { get; set; }
    }
}