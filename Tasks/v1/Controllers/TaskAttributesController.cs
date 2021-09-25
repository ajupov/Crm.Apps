using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Tasks.Models;
using Crm.Apps.Tasks.Services;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Tasks.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireTasksRole(JwtDefaults.AuthenticationScheme)]
    [Route("Tasks/Attributes/v1")]
    public class TaskAttributesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ITaskAttributesService _taskAttributesService;

        public TaskAttributesController(
            IUserContext userContext,
            ITaskAttributesService taskAttributesService)
            : base(userContext)
        {
            _userContext = userContext;
            _taskAttributesService = taskAttributesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<TaskAttribute>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var attribute = await _taskAttributesService.GetAsync(id, false, ct);
            if (attribute == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(attribute, Roles.Tasks, attribute.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<TaskAttribute>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var attributes = await _taskAttributesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(attributes, Roles.Tasks, attributes.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskAttributeGetPagedListResponse>> GetPagedList(
            TaskAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _taskAttributesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Tasks, response.Attributes.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(TaskAttribute attribute, CancellationToken ct = default)
        {
            attribute.AccountId = _userContext.AccountId;

            var id = await _taskAttributesService.CreateAsync(_userContext.UserId, attribute, ct);

            return Created("Get", id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(TaskAttribute attribute, CancellationToken ct = default)
        {
            var oldAttribute = await _taskAttributesService.GetAsync(attribute.Id, true, ct);
            if (oldAttribute == null)
            {
                return NotFound(attribute.Id);
            }

            return await ActionIfAllowed(
                () => _taskAttributesService.UpdateAsync(_userContext.UserId, oldAttribute, attribute, ct),
                Roles.Tasks,
                oldAttribute.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _taskAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _taskAttributesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Tasks,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _taskAttributesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _taskAttributesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Tasks,
                attributes.Select(x => x.AccountId));
        }
    }
}
