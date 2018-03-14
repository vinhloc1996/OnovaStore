using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Notification
    {
        public Notification()
        {
            CustomerNotification = new HashSet<CustomerNotification>();
            StaffNotification = new HashSet<StaffNotification>();
        }

        [Column("NotificationID")]
        public int NotificationId { get; set; }
        [Required]
        [StringLength(100)]
        public string NotificationName { get; set; }
        [Column(TypeName = "ntext")]
        public string NotificationDescription { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ReleaseDate { get; set; }

        [InverseProperty("Notification")]
        public ICollection<CustomerNotification> CustomerNotification { get; set; }
        [InverseProperty("Notification")]
        public ICollection<StaffNotification> StaffNotification { get; set; }
    }
}
