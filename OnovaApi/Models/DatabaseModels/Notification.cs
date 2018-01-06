using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Notification
    {
        public Notification()
        {
            CustomerNotification = new HashSet<CustomerNotification>();
            StaffNotification = new HashSet<StaffNotification>();
        }

        public int NotificationId { get; set; }
        public string NotificationName { get; set; }
        public string NotificationDescription { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public ICollection<CustomerNotification> CustomerNotification { get; set; }
        public ICollection<StaffNotification> StaffNotification { get; set; }
    }
}
