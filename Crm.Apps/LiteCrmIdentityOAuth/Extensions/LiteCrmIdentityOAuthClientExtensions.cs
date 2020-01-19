﻿using System.Linq;
using System.Security.Claims;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.LiteCrmIdentityOAuth.Helpers;
using Crm.Apps.LiteCrmIdentityOAuth.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Crm.Apps.LiteCrmIdentityOAuth.Extensions
{
    public static class LiteCrmIdentityOAuthClientExtensions
    {
        public static AuthenticationBuilder AddLiteCrmIdentityOAuth(
            this AuthenticationBuilder builder,
            IConfiguration configuration)
        {
            var liteCrmOAuthOptions = configuration.GetSection(nameof(LiteCrmIdentityOAuthOptions));

            return builder
                .AddOAuth(JwtDefaults.AuthenticationScheme, options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                        options.ClientId =
                            liteCrmOAuthOptions.GetValue<string>(nameof(LiteCrmIdentityOAuthOptions.ClientId));

                        options.ClientSecret =
                            liteCrmOAuthOptions.GetValue<string>(nameof(LiteCrmIdentityOAuthOptions.ClientSecret));

                        options.Scope.Add(LiteCrmIdentityOAuthDefaults.OpenIdScope);
                        options.Scope.Add(LiteCrmIdentityOAuthDefaults.ProfileScope);

                        liteCrmOAuthOptions
                            .GetSection(nameof(LiteCrmIdentityOAuthOptions.Scopes))?
                            .GetChildren()?
                            .Select(x => x.Value)
                            .ToList()
                            .ForEach(x => options.Scope.Add(x));

                        options.CallbackPath = new PathString(
                            liteCrmOAuthOptions.GetValue<string>(nameof(LiteCrmIdentityOAuthOptions.CallbackPath)));

                        options.AuthorizationEndpoint =
                            liteCrmOAuthOptions.GetValue<string>(nameof(LiteCrmIdentityOAuthOptions
                                .AuthorizationUrl)) ?? LiteCrmIdentityOAuthDefaults.AuthorizationUrl;

                        options.UserInformationEndpoint =
                            liteCrmOAuthOptions.GetValue<string>(nameof(LiteCrmIdentityOAuthOptions.UserInfoUrl)) ??
                            LiteCrmIdentityOAuthDefaults.UserInfoUrl;

                        options.TokenEndpoint =
                            liteCrmOAuthOptions.GetValue<string>(nameof(LiteCrmIdentityOAuthOptions.TokenUrl)) ??
                            LiteCrmIdentityOAuthDefaults.TokenUrl;

                        options.SaveTokens = true;

                        options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, JwtDefaults.IdentifierClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.Email, JwtDefaults.EmailClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.HomePhone, JwtDefaults.PhoneClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.Surname, JwtDefaults.SurnameClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.Name, JwtDefaults.NameClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, JwtDefaults.BirthDateClaimType);
                        options.ClaimActions.MapJsonKey(ClaimTypes.Gender, JwtDefaults.GenderClaimType);

                        options.Events = new OAuthEvents
                        {
                            OnCreatingTicket = async context =>
                            {
                                var userInfoJson = await UserInfoHelper.GetUserInfoJsonAsync(context);

                                context.AppendRolesToClaims(userInfoJson);
                                context.AppendUserInfoToCookies(userInfoJson);
                            }
                        };
                    }
                );
        }
    }
}