namespace Crm.Apps.Extensions
{
    public class LiteCrmOAuthOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; } = LiteCrmOAuthDefaults.Scope;

        public string CallbackPath { get; set; }

        public string AuthorizationUrl { get; set; } = LiteCrmOAuthDefaults.AuthorizationUrl;

        public string UserInfoUrl { get; set; } = LiteCrmOAuthDefaults.UserInfoUrl;

        public string TokenUrl { get; set; } = LiteCrmOAuthDefaults.TokenUrl;
    }
}