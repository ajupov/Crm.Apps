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
    [Route("Tasks/Comments/v1")]
    public class TaskCommentsController : AllowingCheckControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly ITasksService _tasksService;
        private readonly ITaskCommentsService _taskCommentsService;

        public TaskCommentsController(
            IUserContext userContext,
            ITasksService tasksService,
            ITaskCommentsService taskCommentsService)
            : base(userContext)
        {
            _userContext = userContext;
            _tasksService = tasksService;
            _taskCommentsService = taskCommentsService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskCommentGetPagedListResponse>> GetPagedList(
            TaskCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var task = await _tasksService.GetAsync(request.TaskId, false, ct);
            var response = await _taskCommentsService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Tasks, task.AccountId);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(TaskComment comment, CancellationToken ct = default)
        {
            var task = await _tasksService.GetAsync(comment.TaskId, false, ct);

            return await ActionIfAllowed(
                () => _taskCommentsService.CreateAsync(_userContext.UserId, comment, ct),
                Roles.Tasks,
                task.AccountId);
        }
    }
}
