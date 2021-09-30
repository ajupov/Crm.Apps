﻿using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Auth.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Auth.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [Route("Auth")]
    public class AuthController : DefaultApiController
    {
        private readonly AuthSettings _authSettings;

        public AuthController(IOptions<AuthSettings> options)
        {
            _authSettings = options.Value;
        }

        [AllowAnonymous]
        [IgnoreApiDocumentation]
        [HttpGet("Login")]
        public ActionResult Login(string redirectUri)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri ?? _authSettings.AccountUrl
            };

            return Challenge(properties);
        }

        [AllowAnonymous]
        [HttpGet("IsAuthenticated")]
        public async Task<bool> IsAuthenticated()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(JwtDefaults.AuthenticationScheme);

            return authenticateResult.Succeeded && (authenticateResult.Principal?.Identity?.IsAuthenticated ?? false);
        }

        [AllowAnonymous]
        [IgnoreApiDocumentation]
        [HttpGet("Logout")]
        public async Task<ActionResult> Logout(string redirectUri, CancellationToken ct)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(redirectUri ?? _authSettings.SiteUrl);
        }
    }
}
