using System.ComponentModel.DataAnnotations;

namespace OnovaStore.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Password length from 6 to 32 characters.")]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password must be the same.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Full name must in not over 250 characters.")]
        public string FullName { get; set; }
    }
}