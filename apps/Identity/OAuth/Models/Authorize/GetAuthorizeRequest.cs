using System.ComponentModel.DataAnnotations;
using Identity.OAuth.Attributes.Validation;
using Newtonsoft.Json;

namespace Identity.OAuth.Models.Authorize
{
    public class GetAuthorizeRequest
    {
        [Required]
        [ClientIdValidation]
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [Required]
        [ResponseTypeValidation]
        [JsonProperty("response_type")]
        public string ResponseType { get; set; }

        [Required]
        [ScopeValidation]
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [StateValidation]
        [JsonProperty("state")]
        public string State { get; set; }

        [RedirectUriValidation]
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}