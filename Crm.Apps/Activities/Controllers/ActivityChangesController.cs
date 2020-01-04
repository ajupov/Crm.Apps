using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
    [Route("Api/Activities/Changes")]
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
        public async Task<ActionResult<ActivityChange[]>> GetPagedList(
            ActivityChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(request.ActivityId, ct);
            var changes = await _activityChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, activity.AccountId);
        }
    }
}