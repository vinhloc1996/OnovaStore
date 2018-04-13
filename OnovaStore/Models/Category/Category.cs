using System.Collections.Generic;

namespace OnovaStore.Models.Category
{
    public class Category
    {
        public Category()
        {
            Product = new HashSet<Product.Product>();
        }
        
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string CategoryImage { get; set; }
        public int TotalProduct { get; set; }
        public ICollection<Product.Product> Product { get; set; }
    }
}