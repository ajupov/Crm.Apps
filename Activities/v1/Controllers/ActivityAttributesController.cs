using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Jwt;
using Ajupov.Infrastructure.All.Mvc.Attributes;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Services;
using Crm.Apps.Activities.V1.Requests;
using Crm.Apps.Activities.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.Types.AttributeType;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Activities.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireSalesRole(JwtDefaults.AuthenticationScheme)]
    [Route("Activities/Attributes/v1")]
    public class ActivityAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IActivityAttributesService _activityAttributesService;

        public ActivityAttributesController(
            IUserContext userContext,
            IActivityAttributesService activityAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _activityAttributesService = activityAttributesService;
        }

        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, AttributeType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AttributeType>();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ActivityAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _activityAttributesService.GetAsync(id, false, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Sales, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<ActivityAttribute>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                attributes,
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<ActivityAttributeGetPagedListResponse>> GetPagedList(
            ActivityAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _activityAttributesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(
                response,
                Roles.Sales,
                response.Attributes.Select(x => x.AccountId));
        }

        [HttpPut("Create")]
        public async Task<ActionResult<Guid>> Create(ActivityAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _activityAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created("Get", id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(ActivityAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _activityAttributesService.GetAsync(attribute.Id, true, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _activityAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Sales,
                oldAttribute.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _activityAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _activityAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Sales,
                attributes.Select(x => x.AccountId));
        }
    }
}
