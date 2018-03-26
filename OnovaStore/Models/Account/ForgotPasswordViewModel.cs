using System.ComponentModel.DataAnnotations;

namespace OnovaStore.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string CallbackUrl { get; set; }
    }
}