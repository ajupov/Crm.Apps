using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
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
    [Route("Leads/Attributes/Changes/v1")]
    public class LeadAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly ILeadAttributesService _leadAttributesService;
        private readonly ILeadAttributeChangesService _leadAttributeChangesService;

        public LeadAttributeChangesController(
            IUserContext userContext,
            ILeadAttributeChangesService leadAttributeChangesService,
            ILeadAttributesService leadAttributesService)
            : base(userContext)
        {
            _leadAttributeChangesService = leadAttributeChangesService;
            _leadAttributesService = leadAttributesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<LeadAttributeChangeGetPagedListResponse>> GetPagedList(
            LeadAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _leadAttributesService.GetAsync(request.AttributeId, false, ct);
            var response = await _leadAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Leads, attribute.AccountId);
        }
    }
}
