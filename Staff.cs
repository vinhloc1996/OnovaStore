using System;
using System.Collections.Generic;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Staff
    {
        public Staff()
        {
            GeneralImage = new HashSet<GeneralImage>();
            InverseAddByNavigation = new HashSet<Staff>();
            ReviewConfirm = new HashSet<ReviewConfirm>();
            StaffNotification = new HashSet<StaffNotification>();
        }

        public string StaffId { get; set; }
        public DateTime AddDate { get; set; }
        public string AddBy { get; set; }
        public int? UserStatusId { get; set; }
        public double Salary { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public Staff AddByManagerStaff { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public UserStatus UserStatus { get; set; }
        public ICollection<GeneralImage> GeneralImage { get; set; }
        public ICollection<Staff> InverseAddByNavigation { get; set; }
        public ICollection<ReviewConfirm> ReviewConfirm { get; set; }
        public ICollection<StaffNotification> StaffNotification { get; set; }
    }
}
