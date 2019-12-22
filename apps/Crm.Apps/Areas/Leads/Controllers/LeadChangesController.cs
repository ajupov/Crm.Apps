using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.LeadsManagement)]
    [Route("Api/Leads/Changes")]
    public class LeadChangesController : UserContextController
    {
        private readonly ILeadsService _leadsService;
        private readonly ILeadChangesService _leadChangesService;

        public LeadChangesController(
            IUserContext userContext,
            ILeadsService leadsService,
            ILeadChangesService leadChangesService)
            : base(userContext)
        {
            _leadsService = leadsService;
            _leadChangesService = leadChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<LeadChange>>> GetPagedList(
            LeadChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(parameter.LeadId, ct);
            var changes = await _leadChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.LeadsManagement}, lead.AccountId);
        }
    }
}