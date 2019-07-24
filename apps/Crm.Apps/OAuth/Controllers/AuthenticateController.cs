using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.OAuth.Models;
using Crm.Apps.OAuth.Services;
using Crm.Infrastructure.Mvc;
using Crm.Utils.Generator;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.OAuth.Controllers
{
    [ApiController]
    [Route("Api/Authenticate")]
    public class AuthenticateController : DefaultApiController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateController(
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet("GetProviders")]
        public async Task<ActionResult<Dictionary<string, string>>> GetProviders()
        {
            return await _authenticationService.GetProvidersAsync();
        }

        [HttpPost("Challenge")]
        public ActionResult Challenge(
            [FromForm] ChallengeRequest request)
        {
            var state = Generator.GenerateAlphaNumericString(8);
            var callbackRequest = new CallbackRequest(state, request.RedirectUri);
            var callbackUri = Url.Action($"{request.Provider}Callback", callbackRequest);
            var properties = _authenticationService.GetExternalProperties(state, IpAddress, callbackUri);

            return Challenge(properties, request.Provider);
        }

        [HttpGet("CrmCallback")]
        [HttpGet("VkontakteCallback")]
        [HttpGet("OdnoklassnikiCallback")]
        [HttpGet("InstagramCallback")]
        [HttpGet("YandexCallback")]
        [HttpGet("MailRuCallback")]
        public async Task<IActionResult> Callback(
            CallbackRequest request,
            CancellationToken ct)
        {
            var response = await _authenticationService.CallbackAsync(request, User, IpAddress, UserAgent, ct);
            if (!response.IsSuccess)
            {
                return Forbid();
            }

            return Redirect(response.RedirectUri);
        }
    }
}