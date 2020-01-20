using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Api/Activities/Comments")]
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
        public async Task<ActionResult<List<ActivityComment>>> GetPagedList(
            ActivityCommentGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(request.ActivityId, ct);
            var comments = await _activityCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(comments, Roles.Sales, activity.AccountId);
        }

        [HttpPost("Create")]
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