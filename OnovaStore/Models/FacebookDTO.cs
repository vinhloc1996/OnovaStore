using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OnovaStore.Models
{
    public class FacebookAppAccessToken
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

    public class FacebookUserAccessTokenValidation
    {
        public FacebookUserAccessTokenData Data { get; set; }
    }

    public class FacebookUserAccessTokenData
    {
        [JsonProperty("app_id")]
        public long AppId { get; set; }
        public string Type { get; set; }
        public string Application { get; set; }
        [JsonProperty("expires_at")]
        public long ExpiresAt { get; set; }
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }

    public class FacebookUserData
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Password length from 6 to 32 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string PictureUrl { get; set; }
    }

    public class FacebookPasswordConfirm
    {
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Password length from 6 to 32 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}