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
    [Route("Api/Activities/Changes")]
    public class ActivityChangesController : ControllerBase
    {
        private readonly IActivityChangesService _activityChangesService;

        public ActivityChangesController(IActivityChangesService activityChangesService)
        {
            _activityChangesService = activityChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ActivityChange>>> GetPagedList(
            ActivityChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _activityChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}