using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class StaffNotification
    {
        [Column("StaffID")]
        public string StaffId { get; set; }
        [Column("NotificationID")]
        public int NotificationId { get; set; }
        public bool NotifyStatus { get; set; }
        [Required]
        public byte[] LastUpdate { get; set; }

        [ForeignKey("NotificationId")]
        [InverseProperty("StaffNotification")]
        public Notification Notification { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("StaffNotification")]
        public Staff Staff { get; set; }
    }
}
