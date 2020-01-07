using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Utils.All.String;
using Crm.Apps.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Auth.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : DefaultApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login(string redirectUri)
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = !redirectUri.IsEmpty() ? redirectUri : _authService.GetDefaultRedirectUri()
            };

            return Challenge(authenticationProperties);
        }
    }
}