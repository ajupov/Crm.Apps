using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Activities/Types/v1")]
    public class ActivityTypesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivityTypesService _activityTypesService;

        public ActivityTypesController(IUserContext userContext, IActivityTypesService activityTypesService)
            : base(userContext)
        {
            _userContext = userContext;
            _activityTypesService = activityTypesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ActivityType>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var type = await _activityTypesService.GetAsync(id, false, ct);
            if (type == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(type, Roles.Sales, type.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<ActivityType>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var types = await _activityTypesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                types,
                Roles.Sales,
                types.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityTypeGetPagedListResponse>> GetPagedList(
            ActivityTypeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _activityTypesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Sales,
                response.Types.Select(x => x.AccountId));
        }

        [HttpPut("Create")]
        public async Task<ActionResult<Guid>> Create(ActivityType type, CancellationToken ct = default)
        {
            type.AccountId = _userContext.AccountId;

            var id = await _activityTypesService.CreateAsync(_userContext.UserId, type, ct);

            return Created("Get", id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(ActivityType type, CancellationToken ct = default)
        {
            var oldType = await _activityTypesService.GetAsync(type.Id, true, ct);
            if (oldType == null)
            {
                return NotFound(type.Id);
            }

            return await ActionIfAllowed(
                () => _activityTypesService.UpdateAsync(_userContext.UserId, oldType, type, ct),
                Roles.Sales,
                oldType.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityTypesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityTypesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }
    }
}
