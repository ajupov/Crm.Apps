using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [Route("Api/Activities/Statuses/Changes")]
    public class ActivityStatusesChangesController : ControllerBase
    {
        private readonly IActivityStatusChangesService _activityStatusChangesService;

        public ActivityStatusesChangesController(IActivityStatusChangesService activityStatusChangesService)
        {
            _activityStatusChangesService = activityStatusChangesService;
        }

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityStatusChange[]>> GetPagedList(
            ActivityStatusChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return await _activityStatusChangesService.GetPagedListAsync(request, ct);
        }
    }
}