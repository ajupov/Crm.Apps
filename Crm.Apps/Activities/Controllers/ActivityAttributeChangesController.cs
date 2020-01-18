using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Apps.UserContext.Attributes.Roles;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [RequireSalesRole]
    [Route("Api/Activities/Attributes/Changes")]
    public class ActivityAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly IActivityAttributesService _activityAttributesService;
        private readonly IActivityAttributeChangesService _activityAttributeChangesService;

        public ActivityAttributeChangesController(
            IUserContext userContext,
            IActivityAttributesService activityAttributesService,
            IActivityAttributeChangesService activityAttributeChangesService)
            : base(userContext)
        {
            _activityAttributesService = activityAttributesService;
            _activityAttributeChangesService = activityAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityAttributeChange[]>> GetPagedList(
            ActivityAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _activityAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _activityAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Roles.Sales, attribute.AccountId);
        }
    }
}