using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Api.Attributes;
using Ajupov.Infrastructure.All.Jwt;
using Crm.Apps.Tasks.Services;
using Crm.Apps.Tasks.V1.Requests;
using Crm.Apps.Tasks.V1.Responses;
using Crm.Common.All.BaseControllers;
using Crm.Common.All.Roles;
using Crm.Common.All.Roles.Attributes;
using Crm.Common.All.UserContext;
using Microsoft.AspNetCore.Mvc;
using CrmTask = Crm.Apps.Tasks.Models.Task;

namespace Crm.Apps.Tasks.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireTasksRole(JwtDefaults.AuthenticationScheme)]
    [Route("Tasks/v1")]
    public class TasksController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ITasksService _tasksService;

        public TasksController(IUserContext userContext, ITasksService tasksService)
            : base(userContext)
        {
            _userContext = userContext;
            _tasksService = tasksService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<CrmTask>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var task = await _tasksService.GetAsync(id, false, ct);
            if (task == null)
            {
                return NotFound(id);
            }

            return ReturnIfAllowed(task, Roles.Tasks, task.AccountId);
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<List<CrmTask>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            var tasks = await _tasksService.GetListAsync(ids, ct);

            return ReturnIfAllowed(tasks, Roles.Tasks, tasks.Select(x => x.AccountId));
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskGetPagedListResponse>> GetPagedList(
            TaskGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var response = await _tasksService.GetPagedListAsync(_userContext.AccountId, request, ct);

            return ReturnIfAllowed(response, Roles.Tasks, response.Tasks.Select(x => x.AccountId));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(CrmTask task, CancellationToken ct = default)
        {
            task.AccountId = _userContext.AccountId;

            var id = await _tasksService.CreateAsync(_userContext.UserId, task, ct);

            return Created("Get", id);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult> Update(CrmTask task, CancellationToken ct = default)
        {
            var oldTask = await _tasksService.GetAsync(task.Id, true, ct);
            if (oldTask == null)
            {
                return NotFound(task.Id);
            }

            return await ActionIfAllowed(
                () => _tasksService.UpdateAsync(_userContext.UserId, oldTask, task, ct),
                Roles.Tasks,
                oldTask.AccountId);
        }

        [HttpPatch("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var tasks = await _tasksService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _tasksService.DeleteAsync(_userContext.UserId, tasks.Select(x => x.Id), ct),
                Roles.Tasks,
                tasks.Select(x => x.AccountId));
        }

        [HttpPatch("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            var tasks = await _tasksService.GetListAsync(ids, ct);

            return await ActionIfAllowed(
                () => _tasksService.RestoreAsync(_userContext.UserId, tasks.Select(x => x.Id), ct),
                Roles.Tasks,
                tasks.Select(x => x.AccountId));
        }
    }
}
