using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Activities.Models;
using Crm.Apps.Areas.Activities.RequestParameters;
using Crm.Apps.Areas.Activities.Services;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Activities.Controllers
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

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityChange[]>> GetPagedList(
            ActivityChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return await _activityChangesService.GetPagedListAsync(request, ct);
        }
    }
}