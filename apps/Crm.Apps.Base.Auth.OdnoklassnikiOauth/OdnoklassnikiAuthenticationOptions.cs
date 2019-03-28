using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;

namespace Crm.Common.OAuth.Odnoklassniki
{
    public class OdnoklassnikiAuthenticationOptions : OAuthOptions
    {
        public string ApplicationKey { get; set; }
        public string Format { get; set; }
        public string UserInfoMethod { get; set; }

        public OdnoklassnikiAuthenticationOptions()
        {
            ClaimsIssuer = OdnoklassnikiAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(OdnoklassnikiAuthenticationDefaults.CallbackPath);
            AuthorizationEndpoint = OdnoklassnikiAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = OdnoklassnikiAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = OdnoklassnikiAuthenticationDefaults.UserInformationEndpoint;
            Format = OdnoklassnikiAuthenticationDefaults.Format;
            UserInfoMethod = OdnoklassnikiAuthenticationDefaults.UserInfoMethod;

            Scope.Add("VALUABLE_ACCESS");
            Scope.Add("LONG_ACCESS_TOKEN");
            Scope.Add("GET_EMAIL");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "uid");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
            ClaimActions.MapJsonKey("urn:odnoklassniki:pic_1", "pic_1");
            ClaimActions.MapJsonKey("urn:odnoklassniki:pic_1", "pic_2");
            ClaimActions.MapJsonKey("urn:odnoklassniki:pic_3", "pic_3");
        }

        public ISet<string> Fields { get; } = new HashSet<string>
        {
            "uid",
            "email",
            "first_name",
            "last_name",
            "gender",
            "birthday",
            "pic_1",
            "pic_2",
            "pic_3"
        } as ISet<string>;
    }
}