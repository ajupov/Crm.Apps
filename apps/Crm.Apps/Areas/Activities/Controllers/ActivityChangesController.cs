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
    [Route("Api/Activities/Changes")]
    public class ActivityChangesController : UserContextController
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