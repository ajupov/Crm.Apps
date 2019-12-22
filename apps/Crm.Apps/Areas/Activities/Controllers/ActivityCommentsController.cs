using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Activities.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
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
        public async Task<ActionResult<ActivityComment[]>> GetPagedList(
            ActivityCommentGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(request.ActivityId, ct);
            var comments = await _activityCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(comments, new[] {Role.AccountOwning, Role.SalesManagement}, activity.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(ActivityComment comment, CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(comment.ActivityId, ct);

            return await ActionIfAllowed(
                () => _activityCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                new[] {Role.AccountOwning, Role.SalesManagement},
                activity.AccountId);
        }
    }
}