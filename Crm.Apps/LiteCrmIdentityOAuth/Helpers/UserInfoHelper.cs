using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Crm.Apps.LiteCrmIdentityOAuth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json;

namespace Crm.Apps.LiteCrmIdentityOAuth.Helpers
{
    public static class UserInfoHelper
    {
        public static async Task<UserInfoResponse> GetUserInfoAsync(OAuthCreatingTicketContext context)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint)
            {
                Headers =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json)},
                    Authorization =
                        new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, context.AccessToken)
                },
            };

            var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var userInfoJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UserInfoResponse>(userInfoJson);
        }
    }
}