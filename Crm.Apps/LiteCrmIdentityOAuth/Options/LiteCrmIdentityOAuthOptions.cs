namespace Crm.Apps.LiteCrmIdentityOAuth.Options
{
    public class LiteCrmIdentityOAuthOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string[] Scopes { get; set; }

        public string CallbackPath { get; set; }

        public string AuthorizationUrl { get; set; }

        public string UserInfoUrl { get; set; }

        public string TokenUrl { get; set; }
    }
}