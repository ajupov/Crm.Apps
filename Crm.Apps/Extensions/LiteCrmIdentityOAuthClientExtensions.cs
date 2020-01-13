using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.Extensions
{
    public static class LiteCrmIdentityOAuthClientExtensions
    {
        public static AuthenticationBuilder AddLiteCrmOAuth(
            this AuthenticationBuilder builder,
            IConfiguration configuration)
        {
            var liteCrmOAuthOptions = configuration.GetSection(nameof(LiteCrmOAuthOptions));

            return builder
                .AddOAuth(JwtDefaults.Scheme, options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.ClientId = liteCrmOAuthOptions.GetValue<string>("ClientId");
                        options.ClientSecret = liteCrmOAuthOptions.GetValue<string>("ClientSecret");
                        options.Scope.Add(liteCrmOAuthOptions.GetValue<string>("Scope") ?? LiteCrmOAuthDefaults.Scope);
                        options.CallbackPath = new PathString(liteCrmOAuthOptions.GetValue<string>("CallbackPath"));

                        options.AuthorizationEndpoint = liteCrmOAuthOptions.GetValue<string>("AuthorizationUrl") ??
                                                        LiteCrmOAuthDefaults.AuthorizationUrl;

                        options.UserInformationEndpoint = liteCrmOAuthOptions.GetValue<string>("UserInfoUrl") ??
                                                          LiteCrmOAuthDefaults.UserInfoUrl;

                        options.TokenEndpoint = liteCrmOAuthOptions.GetValue<string>("TokenUrl") ??
                                                LiteCrmOAuthDefaults.TokenUrl;

                        options.SaveTokens = true;

                        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, JwtDefaults.IdentifierClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.Email, JwtDefaults.EmailClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.HomePhone, JwtDefaults.PhoneClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.Surname, JwtDefaults.Scheme);
                        options.ClaimActions.MapJsonKey(ClaimTypes.Name, JwtDefaults.NameClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.Gender, JwtDefaults.GenderClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, JwtDefaults.BirthDate);

                        options.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                var userInfoJsonString = await GetUserInfoJsonString(context);
                                var userInfo = JsonDocument.Parse(userInfoJsonString).RootElement;

                                context.RunClaimActions(userInfo);
                            }
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