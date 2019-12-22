using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Mvc;
using Ajupov.Utils.All.Generator;
using Crm.Apps.Areas.Auth.Models;
using Crm.Apps.Areas.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Auth
{
    [ApiController]
    [Route("Api/Auth")]
    public class AuthController : DefaultApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("GetProviders")]
        public async Task<ActionResult<Dictionary<string, string>>> GetProviders()
        {
            return await _authService.GetProvidersAsync();
        }

        [HttpPost("Challenge")]
        public ActionResult Challenge([FromForm] ChallengeRequest request)
        {
            var state = Generator.GenerateAlphaNumericString(8);
            var callbackRequest = new CallbackRequest(state, request.RedirectUri);
            var callbackUri = Url.Action($"{request.Provider}Callback", callbackRequest);
            var properties = _authService.GetExternalProperties(IpAddress, state, callbackUri);

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
            var response = await _authService.CallbackAsync(request, User, IpAddress, ct);
//            if (!response.IsSuccess)
//            {
//                return Forbid();
//            }

            return Redirect(response.RedirectUri);
        }
    }
}