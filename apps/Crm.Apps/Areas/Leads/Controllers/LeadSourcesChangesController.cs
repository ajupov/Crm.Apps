using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
{
    [ApiController]
    [Route("Api/Leads/Sources/Changes")]
    public class LeadSourcesChangesController : ControllerBase
    {
        private readonly ILeadSourceChangesService _leadSourceChangesService;

        public LeadSourcesChangesController(ILeadSourceChangesService leadSourceChangesService)
        {
            _leadSourceChangesService = leadSourceChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<LeadSourceChange>>> GetPagedList(
            LeadSourceChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _leadSourceChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}