using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireActivitiesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Activities/Attributes/Changes/v1")]
    public class ActivityAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly IActivityAttributesService _activityAttributesService;
        private readonly IActivityAttributeChangesService _activityAttributeChangesService;

        public ActivityAttributeChangesController(
            IUserContext userContext,
            IActivityAttributesService activityAttributesService,
            IActivityAttributeChangesService activityAttributeChangesService)
            : base(userContext)
        {
            _activityAttributesService = activityAttributesService;
            _activityAttributeChangesService = activityAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityAttributeChangeGetPagedListResponse>> GetPagedList(
            ActivityAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _activityAttributesService.GetAsync(request.AttributeId, false, ct);
            var response = await _activityAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Activities, attribute.AccountId);
        }
    }
}
