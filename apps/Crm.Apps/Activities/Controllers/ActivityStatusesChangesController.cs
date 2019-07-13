using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Parameters;
using Crm.Apps.Activities.Services;
using Crm.Common.UserContext;
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

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ActivityStatusChange>>> GetPagedList(
            ActivityStatusChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _activityStatusChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}