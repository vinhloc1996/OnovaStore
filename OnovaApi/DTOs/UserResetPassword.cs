using System.ComponentModel.DataAnnotations;

namespace OnovaApi.DTOs
{
    public class UserResetPassword
    {
        public string Id { get; set; }  
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}