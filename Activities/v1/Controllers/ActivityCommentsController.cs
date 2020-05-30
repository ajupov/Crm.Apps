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
    [Route("Activities/Comments/v1")]
    public class ActivityCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivitiesService _activitiesService;
        private readonly IActivityCommentsService _activityCommentsService;

        public ActivityCommentsController(
            IUserContext userContext,
            IActivitiesService activitiesService,
            IActivityCommentsService activityCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _activitiesService = activitiesService;
            _activityCommentsService = activityCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityCommentGetPagedListResponse>> GetPagedList(
            ActivityCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(request.ActivityId, ct);
            var response = await _activityCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Sales, activity.AccountId);
        }

        [HttpPut("Create")]
        public async Task<ActionResult> Create(ActivityComment comment, CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(comment.ActivityId, ct);

            return await ActionIfAllowed(
                () => _activityCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Sales,
                activity.AccountId);
        }
    }
}
