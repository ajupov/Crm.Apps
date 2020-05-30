using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
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
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Activities/Changes/v1")]
    public class ActivityChangesController : AllowingCheckControllerBase
    {
        private readonly IActivitiesService _activitiesService;
        private readonly IActivityChangesService _activityChangesService;

        public ActivityChangesController(
            IUserContext userContext,
            IActivityChangesService activityChangesService,
            IActivitiesService activitiesService)
            : base(userContext)
        {
            _activityChangesService = activityChangesService;
            _activitiesService = activitiesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityChangeGetPagedListResponse>> GetPagedList(
            ActivityChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(request.ActivityId, ct);
            var response = await _activityChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Sales, activity.AccountId);
        }
    }
}
