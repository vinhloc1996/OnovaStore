namespace OnovaStore.Models.Product
{
    public class ProductCart
    {
        public int ProductId { get; set; }
        public double RealPrice { get; set; }
        public double DisplayPrice { get; set; }
        public int CurrentQuantity { get; set; }
    }
}