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
    [Route("Api/Deals/Statuses/Changes")]
    public class DealStatusesChangesController : ControllerBase
    {
        private readonly IDealStatusChangesService _dealStatusChangesService;

        public DealStatusesChangesController(IDealStatusChangesService dealStatusChangesService)
        {
            _dealStatusChangesService = dealStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<DealStatusChange>>> GetPagedList(
            DealStatusChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _dealStatusChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}