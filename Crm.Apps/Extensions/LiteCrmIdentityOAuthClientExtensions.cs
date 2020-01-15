using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Crm.Apps.Extensions
{
    public static class LiteCrmIdentityOAuthClientExtensions
    {
        public static AuthenticationBuilder AddLiteCrmOAuth(
            this AuthenticationBuilder builder,
            IConfiguration configuration)
        {
            var liteCrmOAuthOptions = configuration.GetSection(nameof(LiteCrmIdentityOAuthOptions));

            return builder
                .AddOAuth(JwtDefaults.Scheme, options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.ClientId = liteCrmOAuthOptions.GetValue<string>("ClientId");
                        options.ClientSecret = liteCrmOAuthOptions.GetValue<string>("ClientSecret");
                        options.Scope.Add(liteCrmOAuthOptions.GetValue<string>("Scope") ?? LiteCrmIdentityOAuthDefaults.Scope);
                        options.CallbackPath = new PathString(liteCrmOAuthOptions.GetValue<string>("CallbackPath"));

                        options.AuthorizationEndpoint = liteCrmOAuthOptions.GetValue<string>("AuthorizationUrl") ??
                                                        LiteCrmIdentityOAuthDefaults.AuthorizationUrl;

                        options.UserInformationEndpoint = liteCrmOAuthOptions.GetValue<string>("UserInfoUrl") ??
                                                          LiteCrmIdentityOAuthDefaults.UserInfoUrl;

                        options.TokenEndpoint = liteCrmOAuthOptions.GetValue<string>("TokenUrl") ??
                                                LiteCrmIdentityOAuthDefaults.TokenUrl;
                        

                        options.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                var userInfoJson = await GetUserInfoJsonString(context);
                                var userInfo = JsonConvert.DeserializeObject<LiteCrmIdentityUserInfo>(userInfoJson);
                                if (userInfo.HasError)
                                {
                                    throw new Exception(userInfo.error);
                                }
                                
                                var response = context.HttpContext.Response;

                                var httpOnlyCookieOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    MaxAge = context.ExpiresIn,
                                    SameSite = SameSiteMode.Lax
                                };

                                response.Cookies.Append("username", userInfo.name);
                                response.Cookies.Append("access_token", context.AccessToken, httpOnlyCookieOptions);
                                response.Cookies.Append("refresh_token", context.RefreshToken, httpOnlyCookieOptions);
                            },
                            
                            OnAccessDenied = 
                        };
                    }
                );
        }

        private static async Task<string> GetUserInfoJsonString(OAuthCreatingTicketContext context)
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

            return await response.Content.ReadAsStringAsync();
        }
    }
}