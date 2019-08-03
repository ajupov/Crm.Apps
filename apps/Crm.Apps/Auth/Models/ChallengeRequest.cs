using System.ComponentModel.DataAnnotations;
using Identity.OAuth.Attributes.Validation;

namespace Identity.OAuth.Models
{
    public class ChallengeRequest
    {
        [Required]
        [OAuthProviderValidation]
        public string Provider { get; set; }

        [RedirectUriValidation]
        public string RedirectUri { get; set; }
    }
}