using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Crm.Infrastructure.Mvc;
using Crm.Utils.String;
using Identity.Clients.Services;
using Identity.OAuth.Attributes.Security;
using Identity.OAuth.Models.Authorize;
using Identity.OAuth.Models.Tokens;
using Identity.OAuth.Services;
using Identity.OAuth.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Identity.OAuth.Controllers
{
    [SecurityHeaders]
    [Route("OAuth")]
    public class OAuthController : DefaultMvcController
    {
        private readonly IOAuthService _oauthService;
        private readonly IClientsService _clientsService;

        public OAuthController(IOAuthService oauthService, IClientsService clientsService)
        {
            _oauthService = oauthService;
            _clientsService = clientsService;
        }

        // Show authorize form
        [HttpGet("Authorize")]
        public async Task<ActionResult> Authorize(GetAuthorizeRequest request, CancellationToken ct)
        {
            var client = await _clientsService.GetByClientIdAsync(request.ClientId, ct);
            if (client == null)
            {
                return BadRequest(request.ClientId);
            }

            if (client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
                client.RedirectUriPattern.IsEmpty())
            {
                return Forbid();
            }

            if (!Regex.IsMatch(request.RedirectUri, client.RedirectUriPattern))
            {
                return BadRequest(request.RedirectUri);
            }

            if (_oauthService.IsAuthorized(HttpContext.User))
            {
                return Redirect(request.RedirectUri);
            }

            var model = new AuthorizeViewModel(request.ResponseType, request.RedirectUri);

            return View("~/OAuth/Views/Authorize.cshtml", model);
        }

        // Redirect with code or tokens
        [ValidateAntiForgeryToken]
        [HttpPost("Authorize")]
        public async Task<ActionResult> Authorize([FromForm] PostAuthorizeRequest request, CancellationToken ct)
        {
            var response = await _oauthService.AuthorizeAsync(request, UserAgent, IpAddress, ct);
            if (!response.IsSuccess)
            {
                return Forbid();
            }

            return Redirect(response.RedirectUri);
        }

        // Return new tokens
        [HttpPost("Token")]
        public async Task<ActionResult<TokenResponse>> Token(TokenRequest request, CancellationToken ct)
        {
            var client = await _clientsService.GetByClientIdAsync(request.ClientId, ct);
            if (client == null)
            {
                return BadRequest(request.ClientId);
            }

            if (client.IsLocked || client.IsDeleted || client.ClientSecret.IsEmpty() ||
                client.RedirectUriPattern.IsEmpty())
            {
                return Forbid();
            }

            if (request.ClientSecret != client.ClientSecret)
            {
                return BadRequest(request.ClientSecret);
            }

            if (!Regex.IsMatch(request.RedirectUri, client.RedirectUriPattern))
            {
                return BadRequest(request.RedirectUri);
            }

            if (_oauthService.IsAuthorized(HttpContext.User))
            {
                return Redirect(request.RedirectUri);
            }

            var response = await _oauthService.GetTokenAsync(request, HttpContext.User, UserAgent, IpAddress, ct);
            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return response;
        }
    }
}