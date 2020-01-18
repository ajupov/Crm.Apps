using Crm.Apps.LiteCrmIdentityOAuth.Models;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Crm.Apps.LiteCrmIdentityOAuth.Extensions
{
    public static class LiteCrmIdentityOAuthUserInfoMiddlewareExtensions
    {
        public static void AppendUserInfoToCookies(
            this OAuthCreatingTicketContext context,
            UserInfoResponse userInfoResponse)
        {
            context.Response.Cookies.Append(LiteCrmIdentityOAuthDefaults.UsernameCookiesName, userInfoResponse.name);
        }
    }
}