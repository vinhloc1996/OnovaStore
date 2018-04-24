using System;

namespace OnovaStore.Areas.Manage.Data
{
    public class GetOrdersForStaff
    {
            public int orderId { get; set; }
            public DateTime orderDate { get; set; }
            public string orderTrackingNumber { get; set; }
            public double displayPrice { get; set; }
            public double interest { get; set; }
            public DateTime estimateShippingDate { get; set; }
            public string name { get; set; }
    }
}