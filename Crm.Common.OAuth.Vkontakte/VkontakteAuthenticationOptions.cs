using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace Crm.Libs.OAuth.Vkontakte
{
    public class VkontakteAuthenticationOptions : OAuthOptions
    {
        public VkontakteAuthenticationOptions()
        {
            ClaimsIssuer = VkontakteAuthenticationDefaults.Issuer;

            CallbackPath = new PathString(VkontakteAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = VkontakteAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = VkontakteAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = VkontakteAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Hash, "hash");
            ClaimActions.MapJsonKey("urn:vkontakte:photo:link", "photo");
            ClaimActions.MapJsonKey("urn:vkontakte:photo_thumb:link", "photo_rec");
        }

        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "id",
            "first_name",
            "last_name",
            "photo_rec",
            "photo",
            "hash"
        };

        public string ApiVersion { get; set; } = "5.78";
    }
}