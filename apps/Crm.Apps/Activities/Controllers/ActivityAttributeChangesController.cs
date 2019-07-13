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
    [Route("Api/Activities/Attributes/Changes")]
    public class ActivityAttributeChangesController : ControllerBase
    {
        private readonly IActivityAttributeChangesService _activityAttributeChangesService;

        public ActivityAttributeChangesController(IActivityAttributeChangesService activityAttributeChangesService)
        {
            _activityAttributeChangesService = activityAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<ActivityAttributeChange>>> GetPagedList(
            ActivityAttributeChangeGetPagedListParameter parameter, CancellationToken ct = default)
        {
            return await _activityAttributeChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}