using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Utils.All.String;
using Crm.Apps.RefreshTokens.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Auth.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : DefaultApiController
    {
        private readonly IRefreshTokensService _refreshTokensService;

        public AuthController(IRefreshTokensService refreshTokensService)
        {
            _refreshTokensService = refreshTokensService;
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login(string redirectUri)
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.ActionLink("LoginCallback", "Auth", new
                {
                    redirectUri = !redirectUri.IsEmpty() ? redirectUri : Url.ActionLink("Index", "Home")
                })
            };

            return Challenge(authenticationProperties);
        }

        [AllowAnonymous]
        [HttpGet("LoginCallback")]
        public async Task<IActionResult> LoginCallback(string redirectUri, CancellationToken ct)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(JwtDefaults.Scheme);
            var userId = authenticateResult.Principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var tokens = authenticateResult.Properties.GetTokens().ToList();
            var accessToken = tokens.First(x => x.Name == "access_token").Value;
            var refreshToken = tokens.First(x => x.Name == "refresh_token").Value;

            await _refreshTokensService.CreateAsync(Guid.Parse(userId), accessToken, refreshToken, ct);

            HttpContext.Response.Cookies.Append("username", authenticateResult.Principal.Identity.Name);
            HttpContext.Response.Cookies.Append("access_token", accessToken);

            return Redirect(redirectUri);
        }

        [Authorize]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout(string redirectUri, CancellationToken ct)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(JwtDefaults.Scheme);
            var tokens = authenticateResult.Properties.GetTokens().ToList();
            var accessToken = tokens.First(x => x.Name == "access_token").Value;
            var accessToken2 = await HttpContext.GetTokenAsync("access_token");

            HttpContext.Response.Cookies.Delete("username");
            HttpContext.Response.Cookies.Delete("access_token");

            await _refreshTokensService.SetIsExpiredAsync(accessToken2, ct);

            return Redirect(redirectUri);
        }
    }
}