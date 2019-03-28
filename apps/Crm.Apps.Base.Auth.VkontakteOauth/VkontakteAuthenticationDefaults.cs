namespace Crm.Common.OAuth.Vkontakte
{
    public static class VkontakteAuthenticationDefaults
    {
        public const string AuthenticationScheme = "Vkontakte";
        public const string DisplayName = "Vkontakte";
        public const string Issuer = "Vkontakte";
        public const string CallbackPath = "/signin-vkontakte";
        public const string AuthorizationEndpoint = "https://oauth.vk.com/authorize";
        public const string TokenEndpoint = "https://oauth.vk.com/access_token";
        public const string UserInformationEndpoint = "https://api.vk.com/method/users.get.json";
    }
}