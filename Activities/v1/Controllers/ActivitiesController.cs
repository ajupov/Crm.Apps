using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.v1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Activities/v1")]
    public class ActivitiesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivitiesService _activitiesService;

        public ActivitiesController(IUserContext userContext, IActivitiesService activitiesService)
            : base(userContext)
        {
            _userContext = userContext;
            _activitiesService = activitiesService;
        }

        [HttpGet("GetTypes")]
        public Dictionary<string, ActivityType> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<ActivityType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Activity>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var activity = await _activitiesService.GetAsync(id, ct);
            if (activity == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(activity, Roles.Sales, activity.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<Activity>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                activities,
                Roles.Sales,
                activities.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<Activity>>> GetPagedList(
            ActivityGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            request.AccountId = _userContext.AccountId;

            var activities = await _activitiesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(
                activities,
                Roles.Sales,
                activities.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Activity activity, CancellationToken ct = default)
        {
            activity.AccountId = _userContext.AccountId;

            var id = await _activitiesService.CreateAsync(_userContext.UserId, activity, ct);

            return Created("Get", id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult> Update(Activity activity, CancellationToken ct = default)
        {
            var oldActivity = await _activitiesService.GetAsync(activity.Id, ct);
            if (oldActivity == null)
            {
                return NotFound(activity.Id);
            }

            return await ActionIfAllowed(
                () => _activitiesService.UpdateAsync(_userContext.UserId, oldActivity, activity, ct),
                Roles.Sales,
                activity.AccountId, oldActivity.AccountId);
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activitiesService.DeleteAsync(_userContext.UserId, activities.Select(x => x.Id), ct),
                Roles.Sales,
                activities.Select(x => x.AccountId));
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var activities = await _activitiesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activitiesService.RestoreAsync(_userContext.UserId, activities.Select(x => x.Id), ct),
                Roles.Sales,
                activities.Select(x => x.AccountId));
        }
    }
}