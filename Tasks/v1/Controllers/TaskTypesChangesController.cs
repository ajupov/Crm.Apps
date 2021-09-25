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
    [Route("Tasks/Types/Changes/v1")]
    public class TaskTypesChangesController : AllowingCheckControllerBase
    {
        private readonly ITaskTypesService _taskTypesService;
        private readonly ITaskTypeChangesService _taskTypeChangesService;

        public TaskTypesChangesController(
            IUserContext userContext,
            ITaskTypeChangesService taskTypeChangesService,
            ITaskTypesService taskTypesService)
            : base(userContext)
        {
            _taskTypeChangesService = taskTypeChangesService;
            _taskTypesService = taskTypesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskTypeChangeGetPagedListResponse>> GetPagedList(
            TaskTypeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var type = await _taskTypesService.GetAsync(request.TypeId, false, ct);
            var response = await _taskTypeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Tasks, type.AccountId);
        }
    }
}
