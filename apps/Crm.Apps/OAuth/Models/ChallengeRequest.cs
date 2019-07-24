using System.ComponentModel.DataAnnotations;
using Crm.Apps.OAuth.Attributes;

namespace Crm.Apps.OAuth.Models
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