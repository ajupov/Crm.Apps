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
    [Route("Activities/Types/Changes/v1")]
    public class ActivityTypesChangesController : AllowingCheckControllerBase
    {
        private readonly IActivityTypesService _activityTypesService;
        private readonly IActivityTypeChangesService _activityTypeChangesService;

        public ActivityTypesChangesController(
            IUserContext userContext,
            IActivityTypeChangesService activityTypeChangesService,
            IActivityTypesService activityTypesService)
            : base(userContext)
        {
            _activityTypeChangesService = activityTypeChangesService;
            _activityTypesService = activityTypesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityTypeChangeGetPagedListResponse>> GetPagedList(
            ActivityTypeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var type = await _activityTypesService.GetAsync(request.TypeId, false, ct);
            var response = await _activityTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Activities, type.AccountId);
        }
    }
}
