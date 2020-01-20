using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
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

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<ActivityTypeChange>>> GetPagedList(
            ActivityTypeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var type = await _activityTypesService.GetAsync(request.TypeId, ct);
            var changes = await _activityTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, type.AccountId);
        }
    }
}