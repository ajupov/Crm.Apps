using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Parameters;
using Crm.Apps.Areas.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Common.UserContext.Extensions;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/Users/Groups/Changes")]
    public class UserGroupChangesController : UserContextController
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

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<UserGroupChange>>> GetPagedList(
            UserGroupChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            var group = await _userGroupsService.GetAsync(parameter.GroupId, ct);
            var changes = await _userGroupChangesService.GetPagedListAsync(parameter, ct);

            return ReturnIfAllowed(changes, Role.AccountOwning, group.AccountId);
        }
    }
}