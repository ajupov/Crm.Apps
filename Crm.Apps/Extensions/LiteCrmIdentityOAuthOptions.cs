namespace Crm.Apps.Extensions
{
    public class LiteCrmIdentityOAuthOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; } = LiteCrmIdentityOAuthDefaults.Scope;

        public string CallbackPath { get; set; }

        public string AuthorizationUrl { get; set; } = LiteCrmIdentityOAuthDefaults.AuthorizationUrl;

        public string UserInfoUrl { get; set; } = LiteCrmIdentityOAuthDefaults.UserInfoUrl;

        public string TokenUrl { get; set; } = LiteCrmIdentityOAuthDefaults.TokenUrl;
    }
}