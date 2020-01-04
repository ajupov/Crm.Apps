using System.ComponentModel.DataAnnotations;

namespace Crm.Apps.Auth.Models
{
    public class ChallengeRequest
    {
        [Required]
        [OAuthProviderValidation]
        public string Provider { get; set; }

//        [RedirectUriValidation]
        public string RedirectUri { get; set; }
    }
}