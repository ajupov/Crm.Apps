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
using TaskStatus = Crm.Apps.Tasks.Models.TaskStatus;

namespace Crm.Apps.Tasks.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireTasksRole(JwtDefaults.AuthenticationScheme)]
    [Route("Tasks/Statuses/v1")]
    public class TaskStatusesController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ITaskStatusesService _taskStatusesService;

        public TaskStatusesController(IUserContext userContext, ITaskStatusesService taskStatusesService)
            : base(userContext)
        {
            _userContext = userContext;
            _taskStatusesService = taskStatusesService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<TaskStatus>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var status = await _taskStatusesService.GetAsync(id, false, ct);
            if (status == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(status, Roles.Tasks, status.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<TaskStatus>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var response = await _taskStatusesService.GetListAsync(ids, ct);

            return ReturnIfAllowed(
                response,
                Roles.Tasks,
                response.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskStatusGetPagedListResponse>> GetPagedList(
            TaskStatusGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var statuses = await _taskStatusesService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(statuses, Roles.Tasks, statuses.Statuses.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(TaskStatus status, CancellationToken ct = default)
        {
            status.AccountId = _userContext.AccountId;

            var id = await _taskStatusesService.CreateAsync(_userContext.UserId, status, ct);

            return Created("Get", id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(TaskStatus status, CancellationToken ct = default)
        {
            var oldStatus = await _taskStatusesService.GetAsync(status.Id, true, ct);
            if (oldStatus == null)
            {
                return NotFound(status.Id);
            }

            return await ActionIfAllowed(
                () => _taskStatusesService.UpdateAsync(_userContext.UserId, oldStatus, status, ct),
                Roles.Tasks,
                oldStatus.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _taskStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _taskStatusesService.DeleteAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Tasks,
                attributes.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var attributes = await _taskStatusesService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _taskStatusesService.RestoreAsync(_userContext.UserId, attributes.Select(x => x.Id), ct),
                Roles.Tasks,
                attributes.Select(x => x.AccountId));
        }
    }
}
