using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;
using Crm.Apps.Activities.Services;
using Crm.Common.All.UserContext;
using Crm.Common.All.UserContext.Attributes;
using Crm.Common.All.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.Controllers
{
    [ApiController]
    [RequirePrivileged(Role.AccountOwning, Role.SalesManagement)]
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

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityAttributeChange[]>> GetPagedList(
            ActivityAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var attribute = await _activityAttributesService.GetAsync(request.AttributeId, ct);
            var changes = await _activityAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, new[] {Role.AccountOwning, Role.SalesManagement}, attribute.AccountId);
        }
    }
}