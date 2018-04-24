using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnovaApi.Models.DatabaseModels
{
    public partial class UserStatus
    {
        public UserStatus()
        {
            Staff = new HashSet<Staff>();
        }

        [Column("UserStatusID")]
        public int UserStatusId { get; set; }
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [InverseProperty("UserStatus")]
        public ICollection<Staff> Staff { get; set; }
    }
}
