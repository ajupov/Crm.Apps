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
    [Route("Api/Activities/Statuses/Changes")]
    public class ActivityStatusesChangesController : AllowingCheckControllerBase
    {
        private readonly IActivityStatusesService _activityStatusesService;
        private readonly IActivityStatusChangesService _activityStatusChangesService;

        public ActivityStatusesChangesController(
            IUserContext userContext,
            IActivityStatusesService activityStatusesService,
            IActivityStatusChangesService activityStatusChangesService)
            : base(userContext)
        {
            _activityStatusesService = activityStatusesService;
            _activityStatusChangesService = activityStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityStatusChange[]>> GetPagedList(
            ActivityStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var status = await _activityStatusesService.GetAsync(request.StatusId, ct);
            var changes = await _activityStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, status.AccountId);
        }
    }
}