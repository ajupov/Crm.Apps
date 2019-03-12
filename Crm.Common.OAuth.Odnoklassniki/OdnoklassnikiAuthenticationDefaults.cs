namespace Crm.Libs.OAuth.Odnoklassniki
{
    public class OdnoklassnikiAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Odnoklassniki";
        public const string DisplayName = "Odnoklassniki";
        public const string Issuer = "Odnoklassniki";
        public const string CallbackPath = "/signin-ok";
        public const string AuthorizationEndpoint = "https://connect.ok.ru/oauth/authorize";
        public const string TokenEndpoint = "https://api.ok.ru/oauth/token.do";
        public const string UserInformationEndpoint = "https://api.ok.ru/fb.do";
        public const string Format = "json";
        public const string UserInfoMethod = "users.getCurrentUser";
    }
}