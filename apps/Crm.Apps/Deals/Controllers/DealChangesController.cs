using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Deals.Models;
using Crm.Apps.Deals.Parameters;
using Crm.Apps.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Deals.Controllers
{
    [ApiController]
    [Route("Api/Deals/Changes")]
    public class DealChangesController : ControllerBase
    {
        private readonly IDealChangesService _dealChangesService;

        public DealChangesController(IDealChangesService dealChangesService)
        {
            _dealChangesService = dealChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<DealChange>>> GetPagedList(DealChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _dealChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}