using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Ajupov.Infrastructure.All.Mvc;
using Crm.Apps.Auth.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Auth.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Auth")]
    public class AuthController : DefaultApiController
    {
        private readonly AuthSettings _authSettings;

        public AuthController(IOptions<AuthSettings> options)
        {
            _authSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login(string redirectUri)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUri ?? _authSettings.AccountUrl
            };

            return Challenge(properties);
        }

        [AllowAnonymous]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout(string redirectUri, CancellationToken ct)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(redirectUri ?? _authSettings.SiteUrl);
        }
    }
}