using Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking;

namespace OnovaStore.Areas.Manage.Data
{
    public class GetProductForStaff
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductStatus { get; set; }
        public double Sales { get; set; }
        public int NumberOrder { get; set; }
        public double Rating { get; set; }
        public int WishCounting { get; set; }
    }
}