using System.Collections.Generic;

namespace OnovaStore.Models.Category
{
    public class CategoryProducts
    {
        public int productId { get; set; }
        public double displayPrice { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public object productThumbImage { get; set; }
        public string brandName { get; set; }
        public string statusCode { get; set; }
        public string brandSlug { get; set; }
    }

    public class CategoryInfo
    {
        public int categoryId { get; set; }
        public object categoryImage { get; set; }
        public string name { get; set; }
        public int totalProduct { get; set; }
        public string slug { get; set; }
        public List<CategoryProducts> products { get; set; }
    }

    public class RootCategoryProducts
    {
        public string status { get; set; }
        public string message { get; set; }
        public CategoryInfo categories { get; set; }
    }
}