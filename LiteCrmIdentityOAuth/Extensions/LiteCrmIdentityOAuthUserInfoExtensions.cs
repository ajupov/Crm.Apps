using Crm.Apps.LiteCrmIdentityOAuth.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json;

namespace Crm.Apps.LiteCrmIdentityOAuth.Extensions
{
    public static class LiteCrmIdentityOAuthUserInfoExtensions
    {
        public static void AppendUserInfoToCookies(this OAuthCreatingTicketContext context, string userInfoJson)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfoResponse>(userInfoJson);

            context.Response.Cookies.Append(LiteCrmIdentityOAuthDefaults.UsernameCookiesName, userInfo.name);
        }
    }
}