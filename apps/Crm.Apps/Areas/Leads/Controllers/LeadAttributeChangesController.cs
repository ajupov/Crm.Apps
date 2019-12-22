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
    [Route("Api/Leads/Attributes/Changes")]
    public class LeadAttributeChangesController : ControllerBase
    {
        private readonly ILeadAttributeChangesService _leadAttributeChangesService;

        public LeadAttributeChangesController(ILeadAttributeChangesService leadAttributeChangesService)
        {
            _leadAttributeChangesService = leadAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Role.System, Role.Development, Role.Administration, Role.TechnicalSupport)]
        public async Task<ActionResult<List<LeadAttributeChange>>> GetPagedList(
            LeadAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _leadAttributeChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}