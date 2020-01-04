using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Crm.Apps.Users.Models;
using Crm.Apps.Users.RequestParameters;
using Crm.Apps.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.BaseControllers;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Users.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Groups/Changes")]
    public class UserGroupChangesController : AllowingCheckControllerBase
    {
        private readonly IUserGroupsService _userGroupsService;
        private readonly IUserGroupChangesService _userGroupChangesService;

        public UserGroupChangesController(
            IUserContext userContext,
            IUserGroupsService userGroupsService,
            IUserGroupChangesService userGroupChangesService)
            : base(userContext)
        {
            _userGroupsService = userGroupsService;
            _userGroupChangesService = userGroupChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<UserGroupChange>>> GetPagedList(
            UserGroupChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            var group = await _userGroupsService.GetAsync(request.GroupId, ct);
            var changes = await _userGroupChangesService.GetPagedListAsync(request, ct);

            return ReturnIfAllowed(changes, Role.AccountOwning, group.AccountId);
        }
    }
}