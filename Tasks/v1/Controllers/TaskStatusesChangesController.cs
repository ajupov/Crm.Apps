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
    [Route("Tasks/Statuses/Changes/v1")]
    public class TaskStatusesChangesController : AllowingCheckControllerBase
    {
        private readonly ITaskStatusesService _taskStatusesService;
        private readonly ITaskStatusChangesService _taskStatusChangesService;

        public TaskStatusesChangesController(
            IUserContext userContext,
            ITaskStatusesService taskStatusesService,
            ITaskStatusChangesService taskStatusChangesService)
            : base(userContext)
        {
            _taskStatusesService = taskStatusesService;
            _taskStatusChangesService = taskStatusChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskStatusChangeGetPagedListResponse>> GetPagedList(
            TaskStatusChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var status = await _taskStatusesService.GetAsync(request.StatusId, false, ct);
            var response = await _taskStatusChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Tasks, status.AccountId);
        }
    }
}
