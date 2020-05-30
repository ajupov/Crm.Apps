using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Leads.Services;
using Crm.Apps.Leads.V1.Requests;
using Crm.Apps.Leads.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Leads.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireLeadsRole(JwtDefaults.AuthenticationScheme)]
    [Route("Leads/Sources/Changes/v1")]
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
        public async Task<ActionResult<LeadSourceChangeGetPagedListResponse>> GetPagedList(
            LeadSourceChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var source = await _leadSourcesService.GetAsync(request.SourceId, ct);
            var response = await _leadSourceChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Leads, source.AccountId);
        }
    }
}
