// using System.Threading;
// using System.Threading.Tasks;
// using Ajupov.Infrastructure.All.Mvc;
// using Ajupov.Utils.All.String;
// using Crm.Apps.RefreshTokens.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Crm.Apps.Auth.Controllers
// {
//     [ApiController]
//     [Route("Auth")]
//     public class AuthController : DefaultApiController
//     {
//         private readonly IRefreshTokensService _refreshTokensService;
//         private readonly IHttpContextAccessor _httpContextAccessor;
//
//         public AuthController(IRefreshTokensService refreshTokensService, IHttpContextAccessor httpContextAccessor)
//         {
//             _refreshTokensService = refreshTokensService;
//             _httpContextAccessor = httpContextAccessor;
//         }
//
//         // [AllowAnonymous]
//         // [HttpGet("Login")]
//         // public IActionResult Login(string redirectUri)
//         // {
//         //     var authenticationProperties = new AuthenticationProperties
//         //     {
//         //         RedirectUri = redirectUri
//         //         // RedirectUri = Url.Action("LoginCallback", "Auth", new
//         //         // {
//         //         //     redirectUri = GetCorrectRedirectUri(redirectUri)
//         //         // })
//         //     };
//         //
//         //     return Challenge(authenticationProperties);
//         // }
//
//         // [AllowAnonymous]
//         // [HttpGet("LoginCallback")]
//         // public async Task<IActionResult> LoginCallback(string redirectUri, CancellationToken ct)
//         // {
//         //     var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(JwtDefaults.Scheme);
//         //     var userId = authenticateResult.Principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
//         //     var tokens = authenticateResult.Properties.GetTokens().ToList();
//         //     var accessToken = tokens.First(x => x.Name == "access_token").Value;
//         //     var refreshToken = tokens.First(x => x.Name == "refresh_token").Value;
//         //
//         //     await _refreshTokensService.CreateAsync(Guid.Parse(userId), accessToken, refreshToken, ct);
//         //
//         //     _httpContextAccessor.HttpContext.Response.Cookies.Append("username",
//         //         authenticateResult.Principal.Identity.Name);
//         //
//         //     _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", accessToken,
//         //         new CookieOptions {HttpOnly = true, SameSite = SameSiteMode.Lax, MaxAge = TimeSpan.FromMinutes(10)});
//         //
//         //     return Redirect(redirectUri);
//         // }
//
//         [Authorize]
//         [HttpPost("Logout")]
//         [HttpGet("Logout")]
//         public async Task<IActionResult> Logout(string redirectUri, CancellationToken ct)
//         {
//             // var accessToken = await HttpContext.GetTokenAsync("access_token");
//
//             _httpContextAccessor.HttpContext.Response.Cookies.Delete("refresh_token");    
//             _httpContextAccessor.HttpContext.Response.Cookies.Delete("access_token");
//             _httpContextAccessor.HttpContext.Response.Cookies.Delete("username");
//
//             // await _refreshTokensService.SetIsExpiredAsync(accessToken, ct);
//
//             return Redirect(GetCorrectRedirectUri(redirectUri));
//         }
//
//         [NonAction]
//         private string GetCorrectRedirectUri(string redirectUri)
//         {
//             return !redirectUri.IsEmpty() ? redirectUri : Url.ActionLink("Index", "Home");
//         }
//     }
// }