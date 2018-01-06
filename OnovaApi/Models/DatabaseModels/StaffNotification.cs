using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class StaffNotification
    {
        public string StaffId { get; set; }
        public int NotificationId { get; set; }
        public bool NotifyStatus { get; set; }
        public byte[] LastUpdate { get; set; }

        public Notification Notification { get; set; }
        public Staff Staff { get; set; }
    }
}
