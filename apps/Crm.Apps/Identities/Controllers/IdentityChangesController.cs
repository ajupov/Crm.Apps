using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Parameters;
using Crm.Apps.Identities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Identities.Controllers
{
    [ApiController]
    [Route("Api/Identities/Changes")]
    public class IdentityChangesController : ControllerBase
    {
        private readonly IIdentityChangesService _identityChangesService;

        public IdentityChangesController(IIdentityChangesService identityChangesService)
        {
            _identityChangesService = identityChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<IdentityChange>>> GetPagedList(
            IdentityChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _identityChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}