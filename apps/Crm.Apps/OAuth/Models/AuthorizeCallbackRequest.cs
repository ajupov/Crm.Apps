using System.ComponentModel.DataAnnotations;
using Crm.Apps.OAuth.Attributes;
using Newtonsoft.Json;

namespace Crm.Apps.OAuth.Models
{
    public class AuthorizeCallbackRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [Required]
        [TokenValidation]
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [Required]
        [TokenValidation]
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [Required]
        [TokenTypeValidation]
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [Required]
        [ExpiresInValidation]
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}