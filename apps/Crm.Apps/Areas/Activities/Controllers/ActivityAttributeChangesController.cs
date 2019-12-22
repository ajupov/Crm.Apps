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
    [Route("Api/Activities/Attributes/Changes")]
    public class ActivityAttributeChangesController : ControllerBase
    {
        private readonly IActivityAttributeChangesService _activityAttributeChangesService;

        public ActivityAttributeChangesController(IActivityAttributeChangesService activityAttributeChangesService)
        {
            _activityAttributeChangesService = activityAttributeChangesService;
        }

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityAttributeChange[]>> GetPagedList(
            ActivityAttributeChangeGetPagedListRequest request, CancellationToken ct = default)
        {
            return await _activityAttributeChangesService.GetPagedListAsync(request, ct);
        }
    }
}