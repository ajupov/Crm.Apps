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
    [Route("Api/Activities/Types/Changes")]
    public class ActivityTypesChangesController : ControllerBase
    {
        private readonly IActivityTypeChangesService _activityTypeChangesService;

        public ActivityTypesChangesController(IActivityTypeChangesService activityTypeChangesService)
        {
            _activityTypeChangesService = activityTypeChangesService;
        }

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityTypeChange[]>> GetPagedList(
            ActivityTypeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return await _activityTypeChangesService.GetPagedListAsync(request, ct);
        }
    }
}