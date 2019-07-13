using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Utils.Guid;
using Crm.Utils.String;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Identities.Controllers
{
    [ApiController]
    [Route("Api/Identities/Tokens")]
    public class IdentityTokensController : ControllerBase
    {
        private readonly IIdentityTokensService _identityTokensService;

        public IdentityTokensController(IIdentityTokensService identityTokensService)
        {
            _identityTokensService = identityTokensService;
        }

        [HttpGet("Get")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<IdentityToken>> Get(Guid identityId, string value,
            CancellationToken ct = default)
        {
            if (identityId.IsEmpty() || value.IsEmpty())
            {
                return BadRequest();
            }

            var token = await _identityTokensService.GetAsync(identityId, value, ct);
            if (token == null)
            {
                return NotFound();
            }

            return token;
        }

        [HttpPost("Create")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult<Guid>> Create(IdentityToken token, CancellationToken ct = default)
        {
            var id = await _identityTokensService.CreateAsync(token, ct);

            return Created(nameof(Get), id);
        }

        [HttpPost("SetIsUsed")]
        [RequireAny(Permission.System, Permission.Development)]
        public async Task<ActionResult> SetIsUsed([FromBody] Guid id, CancellationToken ct = default)
        {
            if (id.IsEmpty())
            {
                return BadRequest();
            }

            await _identityTokensService.SetIsUsedAsync(id, ct);

            return NoContent();
        }
    }
}