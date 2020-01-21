using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.v1.Models;
using Crm.Apps.Leads.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.v1.Controllers
{
    [ApiController]
    [RequireLeadsRole(JwtDefaults.AuthenticationScheme)]
    [Route("api/v1/Leads/Changes")]
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

            return ReturnIfAllowed(changes, Roles.Leads, lead.AccountId);
        }
    }
}