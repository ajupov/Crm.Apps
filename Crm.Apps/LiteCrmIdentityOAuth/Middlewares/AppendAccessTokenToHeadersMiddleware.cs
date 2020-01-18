using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Crm.Apps.LiteCrmIdentityOAuth.Middlewares
{
    public class AppendAccessTokenToHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDataProtector _dataProtector;

        public AppendAccessTokenToHeadersMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider)
        {
            _next = next;
            _dataProtector = dataProtectionProvider.CreateProtector(LiteCrmIdentityOAuthDefaults.DataProtectorName);
        }

        public Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return _next(context);
            }

            if (!context.Request.Cookies.TryGetValue(LiteCrmIdentityOAuthDefaults.SecuredCookiesName, out var tokens))
            {
                return _next(context);
            }

            var accessToken = _dataProtector.Unprotect(tokens).Split(" ").First();

            var authorizationHeaderValue = $"{JwtBearerDefaults.AuthenticationScheme} {accessToken}";
            context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeaderValue);

            return _next(context);
        }
    }
}