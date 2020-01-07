using Crm.Apps.Auth.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthSettings _settings;

        public AuthService(IOptions<AuthSettings> options)
        {
            _settings = options.Value;
        }

        public string GetDefaultRedirectUri()
        {
            return _settings.DefaultRedirectUri;
        }
    }
}