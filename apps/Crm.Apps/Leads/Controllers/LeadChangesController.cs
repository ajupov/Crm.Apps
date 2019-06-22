using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;
using Crm.Apps.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [Route("Api/Leads/Changes")]
    public class LeadChangesController : ControllerBase
    {
        private readonly ILeadChangesService _leadChangesService;

        public LeadChangesController(ILeadChangesService leadChangesService)
        {
            _leadChangesService = leadChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<LeadChange>>> GetPagedList(LeadChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _leadChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}