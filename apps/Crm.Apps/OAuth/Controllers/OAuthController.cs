using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.OAuth.Models;
using Crm.Apps.OAuth.Services;
using Crm.Infrastructure.Mvc;
using Crm.Utils.String;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.OAuth.Controllers
{
    [Route("Api/OAuth")]
    public class OAuthController : DefaultMvcController
    {
        private readonly IOAuthService _oauthService;
        private readonly IOAuthClientsService _oauthClientsService;

        public OAuthController(
            IOAuthService oauthService,
            IOAuthClientsService oauthClientsService)
        {
            _oauthService = oauthService;
            _oauthClientsService = oauthClientsService;
        }

        // Show authorize form
        [HttpGet("Authorize")]
        public async Task<ActionResult> Authorize(
            GetAuthorizeRequest request,
            CancellationToken ct)
        {
            var client = await _oauthClientsService.GetAsync(request.ClientId, ct);
            if (client == null)
            {
                return BadRequest(request.ClientId);
            }

            if (client.IsLocked || client.IsDeleted || client.Secret.IsEmpty() || client.RedirectUriPattern.IsEmpty())
            {
                return Forbid();
            }

            if (!Regex.IsMatch(request.RedirectUri, client.RedirectUriPattern))
            {
                return BadRequest(request.RedirectUri);
            }

            var response = new GetAuthorizeResponse(request.ResponseType, request.RedirectUri);

            return View("~/OAuth/Views/OAuth/Authorize.cshtml", response);
        }

        // Redirect with code or tokens
        [HttpPost("Authorize")]
        public async Task<ActionResult> Authorize(
            [FromForm] PostAuthorizeRequest request,
            CancellationToken ct)
        {
            var response = await _oauthService.AuthorizeAsync(request, UserAgent, IpAddress, ct);
            if (!response.IsSuccess)
            {
                return Forbid();
            }

            return Redirect(response.RedirectUri);
        }

        // Redirect new tokens
        [HttpPost("Token")]
        public async Task<ActionResult<TokenResponse>> Token(
            [FromForm] TokenRequest request,
            CancellationToken ct)
        {
            var response = await _oauthService.GetTokenAsync(request, UserAgent, IpAddress, ct);

            return response;
        }
    }
}