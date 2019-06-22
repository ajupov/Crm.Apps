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
    [Route("Api/Leads/Attributes/Changes")]
    public class LeadAttributeChangesController : ControllerBase
    {
        private readonly ILeadAttributeChangesService _leadAttributeChangesService;

        public LeadAttributeChangesController(ILeadAttributeChangesService leadAttributeChangesService)
        {
            _leadAttributeChangesService = leadAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<LeadAttributeChange>>> GetPagedList(
            LeadAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _leadAttributeChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}