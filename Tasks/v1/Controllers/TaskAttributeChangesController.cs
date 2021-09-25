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
    [Route("Tasks/Attributes/Changes/v1")]
    public class TaskAttributeChangesController : AllowingCheckControllerBase
    {
        private readonly ITaskAttributesService _taskAttributesService;
        private readonly ITaskAttributeChangesService _taskAttributeChangesService;

        public TaskAttributeChangesController(
            IUserContext userContext,
            ITaskAttributesService taskAttributesService,
            ITaskAttributeChangesService taskAttributeChangesService)
            : base(userContext)
        {
            _taskAttributesService = taskAttributesService;
            _taskAttributeChangesService = taskAttributeChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<TaskAttributeChangeGetPagedListResponse>> GetPagedList(
            TaskAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            var attribute = await _taskAttributesService.GetAsync(request.AttributeId, false, ct);
            var response = await _taskAttributeChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(response, Roles.Tasks, attribute.AccountId);
        }
    }
}
