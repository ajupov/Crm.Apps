namespace Crm.Apps.Auth.Settings
{
    public class AuthSettings
    {
        public string AuthorizationUrl { get; set; }
        
        public string TokenUrl { get; set; }

        public string ClientId { get; set; }
        
        public string ClientSecret { get; set; }

        public string ResponseType { get; set; }

        public string Scope { get; set; }

        public string DefaultRedirectUri { get; set; }
        
        public string CallbackPath { get; set; }
    }
}