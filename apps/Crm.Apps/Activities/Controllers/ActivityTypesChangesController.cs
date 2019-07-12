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
    [Route("Api/Activities/Types/Changes")]
    public class ActivityTypesChangesController : ControllerBase
    {
        private readonly IActivityTypeChangesService _activityTypeChangesService;

        public ActivityTypesChangesController(IActivityTypeChangesService activityTypeChangesService)
        {
            _activityTypeChangesService = activityTypeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ActivityTypeChange>>> GetPagedList(
            ActivityTypeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _activityTypeChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}