using Ajupov.Utils.All.String;

namespace Crm.Apps.Auth.Services
{
    public class AuthService : IAuthService
    {
        public string GetCorrectRedirectUri(string redirectUri)
        {
            return !redirectUri.IsEmpty() ? redirectUri : "http://litecrm.org/about";
        }
    }
}