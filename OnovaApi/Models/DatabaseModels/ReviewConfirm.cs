using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class ReviewConfirm
    {
        [Key]
        [Column("ReviewID")]
        public int ReviewId { get; set; }
        public bool? IsApproved { get; set; }
        [Column("AssignStaffID")]
        [StringLength(450)]
        public string AssignStaffId { get; set; }
        [StringLength(256)]
        public string StaffComment { get; set; }
        [Required]
        public byte[] LastUpdateDate { get; set; }

        [ForeignKey("AssignStaffId")]
        [InverseProperty("ReviewConfirm")]
        public Staff AssignStaff { get; set; }
        [ForeignKey("ReviewId")]
        [InverseProperty("ReviewConfirm")]
        public Review Review { get; set; }
    }
}
