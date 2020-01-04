using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [Route("Api/Activities/Types/Changes")]
    public class ActivityTypesChangesController : AllowingCheckControllerBase
    {
        private readonly IActivityTypesService _activityTypesService;
        private readonly IActivityTypeChangesService _activityTypeChangesService;

        public ActivityTypesChangesController(
            IUserContext userContext,
            IActivityTypeChangesService activityTypeChangesService,
            IActivityTypesService activityTypesService)
            : base(userContext)
        {
            _activityTypeChangesService = activityTypeChangesService;
            _activityTypesService = activityTypesService;
        }

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityTypeChange[]>> GetPagedList(
            ActivityTypeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var type = await _activityTypesService.GetAsync(request.TypeId, ct);
            var changes = await _activityTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, type.AccountId);
        }
    }
}