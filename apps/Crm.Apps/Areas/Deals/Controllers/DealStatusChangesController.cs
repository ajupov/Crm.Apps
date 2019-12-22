using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Deals.Models;
using Crm.Apps.Areas.Deals.Parameters;
using Crm.Apps.Areas.Deals.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Deals.Controllers
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
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<DealStatusChange>>> GetPagedList(
            DealStatusChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _dealStatusChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}