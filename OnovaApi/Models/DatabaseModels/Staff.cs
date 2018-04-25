using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnovaApi.Models.IdentityModels;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class Staff
    {
        public Staff()
        {
            GeneralImage = new HashSet<GeneralImage>();
            InverseAddByStaffManager = new HashSet<Staff>();
        }

        [Column("StaffID")]
        public string StaffId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime AddDate { get; set; }
        [StringLength(450)]
        public string AddBy { get; set; }
        [Column("UserStatusID")]
        public int? UserStatusId { get; set; }
        public double Salary { get; set; }
        [Required]
        [StringLength(256)]
        public string Address { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [ForeignKey("AddBy")]
        [InverseProperty("InverseAddByStaffManager")]
        public Staff AddByStaffManager { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("Staff")]
        public ApplicationUser ApplicationUser { get; set; }
        [InverseProperty("Staff")]
        public ICollection<GeneralImage> GeneralImage { get; set; }
        [InverseProperty("AddByStaffManager")]
        public ICollection<Staff> InverseAddByStaffManager { get; set; }
    }
}
