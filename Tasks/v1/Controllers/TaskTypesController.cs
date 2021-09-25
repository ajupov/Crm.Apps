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
    [Route("Tasks/Types/v1")]
    public class TaskTypesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ITaskTypesService _taskTypesService;

        public TaskTypesController(IUserContext userContext, ITaskTypesService taskTypesService)
            : base(userContext)
        {
            _userContext = userContext;
            _taskTypesService = taskTypesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<TaskType>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var type = await _taskTypesService.GetAsync(id, false, ct);
            if (type == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(type, Roles.Tasks, type.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<TaskType>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var types = await _taskTypesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(types, Roles.Tasks, types.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskTypeGetPagedListResponse>> GetPagedList(
            TaskTypeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _taskTypesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Tasks, response.Types.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(TaskType type, CancellationToken ct = default)
        {
            type.AccountId = _userContext.AccountId;

            var id = await _taskTypesService.CreateAsync(_userContext.UserId, type, ct);

            return Created("Get", id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(TaskType type, CancellationToken ct = default)
        {
            var oldType = await _taskTypesService.GetAsync(type.Id, true, ct);
            if (oldType == null)
            {
                return NotFound(type.Id);
            }

            return await ActionIfAllowed(
                () => _taskTypesService.UpdateAsync(_userContext.UserId, oldType, type, ct),
                Roles.Tasks,
                oldType.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _taskTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _taskTypesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Tasks,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _taskTypesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _taskTypesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Tasks,
                attributes.Select(x => x.AccountId));
        }
    }
}
