using System.ComponentModel.DataAnnotations;
using Crm.Apps.OAuth.Attributes;
using Newtonsoft.Json;

namespace Crm.Apps.OAuth.Models
{
    public class GetAuthorizeRequest
    {
        [Required]
        [ResponseTypeValidation]
        [JsonProperty("response_type")]
        public string ResponseType { get; set; }

        [Required]
        [ClientIdValidation]
        [JsonProperty("client_id")]
        public int ClientId { get; set; }

        [Required]
        [ScopeValidation]
        [JsonProperty("scope")]
        public string Scope { get; set; }

        [RedirectUriWithStateValidation]
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}