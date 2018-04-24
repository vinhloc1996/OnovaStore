namespace OnovaApi.DTOs
{
    public class OrderDTO
    {
        public string CartId { get; set; }
        public string TypeUser { get; set; }
        public double? ShippingFee { get; set; }
        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
//        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TokenId { get; set; }
        public double TotalPrice { get; set; }
    }
}