using System.ComponentModel.DataAnnotations;

namespace OnovaApi.DTOs
{
    public class UserForgotPassword
    {
        public string Email { get; set; }
        public string CallbackUrl { get; set; }
    }
}