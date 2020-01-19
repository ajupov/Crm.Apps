using Crm.Apps.LiteCrmIdentityOAuth.Middlewares;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace Crm.Apps.LiteCrmIdentityOAuth.Extensions
{
    public static class LiteCrmIdentityOAuthTokensExtensions
    {
        public static IApplicationBuilder UseAppendAccessTokenToHeaders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppendAccessTokenToHeadersMiddleware>();
        }

        public static void AppendTokensToCookiesAsync(
            this OAuthCreatingTicketContext context,
            IDataProtectionProvider dataProtectionProvider)
        {
            var tokens = dataProtectionProvider
                .CreateProtector(LiteCrmIdentityOAuthDefaults.DataProtectorName)
                .Protect($"{context.AccessToken} {context.RefreshToken}");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax
            };

            context.Response.Cookies.Append(LiteCrmIdentityOAuthDefaults.SecuredCookiesName, tokens, cookieOptions);
        }
    }
}