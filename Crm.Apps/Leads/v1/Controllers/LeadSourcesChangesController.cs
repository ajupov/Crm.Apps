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
    [Route("v1/Leads/Sources/Changes")]
    public class LeadSourcesChangesController : AllowingCheckControllerBase
    {
        private readonly ILeadSourcesService _leadSourcesService;
        private readonly ILeadSourceChangesService _leadSourceChangesService;

        public LeadSourcesChangesController(
            IUserContext userContext,
            ILeadSourcesService leadSourcesService,
            ILeadSourceChangesService leadSourceChangesService)
            : base(userContext)
        {
            _leadSourcesService = leadSourcesService;
            _leadSourceChangesService = leadSourceChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<LeadSourceChange>>> GetPagedList(
            LeadSourceChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var source = await _leadSourcesService.GetAsync(request.SourceId, ct);
            var changes = await _leadSourceChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Leads, source.AccountId);
        }
    }
}