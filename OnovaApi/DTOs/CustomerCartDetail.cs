namespace OnovaApi.DTOs
{
    public class CustomerCartDetail
    {
        public string CustomerCartId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public double DisplayPrice { get; set; }
        public int Quantity { get; set; }
    }
}