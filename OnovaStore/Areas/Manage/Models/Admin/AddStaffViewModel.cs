using System;
using System.ComponentModel.DataAnnotations;

namespace OnovaStore.Areas.Manage.Models.Admin
{
    public class AddStaffViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Password length from 6 to 32 characters.")]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public DateTime AddDate { get; set; } = DateTime.Now;
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public double Salary { get; set; }
        [Required]
        public string FullName { get; set; }
    }
}