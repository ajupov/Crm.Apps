using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Activities.Models;
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
            var type = await _activityTypesService.GetAsync(request.TypeId, ct);
            var response = await _activityTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Sales, type.AccountId);
        }
    }
}
