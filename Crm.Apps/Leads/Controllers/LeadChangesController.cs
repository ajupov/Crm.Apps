using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;
using Crm.Apps.Leads.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.LeadsManagement)]
    [Route("Api/Leads/Changes")]
    public class LeadChangesController : AllowingCheckControllerBase
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
            LeadChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(request.LeadId, ct);
            var changes = await _leadChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.LeadsManagement}, lead.AccountId);
        }
    }
}