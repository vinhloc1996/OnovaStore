using System.Collections.Generic;

namespace OnovaStore.Models.Product
{
    public class Product
    {
        public Product()
        {
        }
        
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double DisplayPrice { get; set; }
        public string ProductThumbImage { get; set; }
        public string Slug { get; set; }
    }
}