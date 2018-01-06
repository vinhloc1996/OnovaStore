using System;
using System.Collections.Generic;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ReviewConfirm
    {
        public int ReviewId { get; set; }
        public bool? IsApproved { get; set; }
        public string AssignStaffId { get; set; }
        public string StaffComment { get; set; }
        public byte[] LastUpdateDate { get; set; }

        public Staff AssignStaff { get; set; }
        public Review Review { get; set; }
    }
}
