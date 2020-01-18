using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;
using Crm.Apps.Leads.Services;
using Crm.Apps.UserContext.Attributes.Roles;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.Controllers
{
    [ApiController]
    [RequireLeadsRole]
    [Route("Api/Leads/Sources/Changes")]
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