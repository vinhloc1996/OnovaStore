using System;
using System.ComponentModel.DataAnnotations;

namespace OnovaApi.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Password length from 6 to 32 characters.")]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; } = DateTime.Now;
    }
}