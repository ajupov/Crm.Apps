using Crm.Apps.OAuth.Attributes;
using Newtonsoft.Json;

namespace Crm.Apps.OAuth.Models
{
    public class TokenRequest
    {
        [GrandTypeValidation]
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [ClientIdValidation]
        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        [ClientSecretValidation]
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [CodeValidation]
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [RedirectUriWithStateValidation]
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}