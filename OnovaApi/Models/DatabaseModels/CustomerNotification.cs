using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class CustomerNotification
    {
        [Column("CustomerID")]
        public string CustomerId { get; set; }
        [Column("NotificationID")]
        public int NotificationId { get; set; }
        public bool NotifyStatus { get; set; }
        [Required]
        public byte[] LastUpdate { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerNotification")]
        public Customer Customer { get; set; }
        [ForeignKey("NotificationId")]
        [InverseProperty("CustomerNotification")]
        public Notification Notification { get; set; }
    }
}
