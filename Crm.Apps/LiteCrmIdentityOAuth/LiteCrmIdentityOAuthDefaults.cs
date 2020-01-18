namespace Crm.Apps.LiteCrmIdentityOAuth
{
    public static class LiteCrmIdentityOAuthDefaults
    {
        public const string OpenIdScope = "openid";
        public const string ProfileScope = "profile";

        public const string AuthorizationUrl = "http://identity.litecrm.org/oauth/authorize";
        public const string UserInfoUrl = "http://identity.litecrm.org/oauth/userinfo";
        public const string TokenUrl = "http://identity.litecrm.org/OAuth/token";

        public const string DataProtectorName = "LiteCrm.DataProtector";

        public const string SecuredCookiesName = ".LiteCrm.Cookies";
        public const string UsernameCookiesName = ".LiteCrm.Username";
    }
}