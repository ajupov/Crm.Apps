using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.RequestParameters;
using Crm.Apps.Areas.Leads.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Leads.Controllers
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