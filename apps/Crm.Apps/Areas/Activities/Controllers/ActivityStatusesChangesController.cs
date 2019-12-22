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