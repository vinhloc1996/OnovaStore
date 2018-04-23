using System.ComponentModel.DataAnnotations;

namespace OnovaStore.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Password length from 6 to 32 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Password length from 6 to 32 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}