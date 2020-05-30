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
    [Route("Leads/Changes/v1")]
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
        public async Task<ActionResult<LeadChangeGetPagedListResponse>> GetPagedList(
            LeadChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var lead = await _leadsService.GetAsync(request.LeadId, ct);
            var response = await _leadChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Leads, lead.AccountId);
        }
    }
}
