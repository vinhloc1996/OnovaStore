using System.Collections.Generic;

namespace OnovaStore.Models.Product
{
    public class ProductSearch
    {
        public int productId { get; set; }
        public string productThumbImage { get; set; }
        public string statusCode { get; set; }
        public string categoryName { get; set; }
        public string name { get; set; }
        public double displayPrice { get; set; }
        public string slug { get; set; }
        public string categorySlug { get; set; }
    }

    public class RootProductSearch
    {
        public string status { get; set; }
        public string message { get; set; }
        public List<ProductSearch> products { get; set; }
    }
}