using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerNotification
    {
        public string CustomerId { get; set; }
        public int NotificationId { get; set; }
        public bool NotifyStatus { get; set; }
        public byte[] LastUpdate { get; set; }

        public Customer Customer { get; set; }
        public Notification Notification { get; set; }
    }
}
