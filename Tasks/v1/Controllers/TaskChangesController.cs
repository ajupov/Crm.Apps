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

namespace Crm.Apps.Tasks.V1.Controllers
{
    [ApiController]
    [RequestContentTypeApplicationJson]
    [ResponseContentTypeApplicationJson]
    [RequireTasksRole(JwtDefaults.AuthenticationScheme)]
    [Route("Tasks/Changes/v1")]
    public class TaskChangesController : AllowingCheckControllerBase
    {
        private readonly ITasksService _tasksService;
        private readonly ITaskChangesService _taskChangesService;

        public TaskChangesController(
            IUserContext userContext,
            ITaskChangesService taskChangesService,
            ITasksService tasksService)
            : base(userContext)
        {
            _taskChangesService = taskChangesService;
            _tasksService = tasksService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskChangeGetPagedListResponse>> GetPagedList(
            TaskChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var task = await _tasksService.GetAsync(request.TaskId, false, ct);
            var response = await _taskChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Tasks, task.AccountId);
        }
    }
}
