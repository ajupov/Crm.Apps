using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Users.Models;
using Crm.Apps.Areas.Users.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Users.Controllers
{
    [ApiController]
    [Route("Api/Users/Groups/Changes")]
    public class UserGroupChangesController : ControllerBase
    {
        private readonly IUserGroupChangesService _userGroupChangesService;

        public UserGroupChangesController(IUserGroupChangesService userGroupChangesService)
        {
            _userGroupChangesService = userGroupChangesService;
        }

        [HttpGet("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<UserGroupChange>>> GetPagedList(
            [FromQuery] Guid? changerUserId = default,
            [FromQuery] Guid? groupId = default,
            [FromQuery] DateTime? minCreateDate = default,
            [FromQuery] DateTime? maxCreateDate = default,
            [FromQuery] int offset = default,
            [FromQuery] int limit = 10,
            [FromQuery] string sortBy = default,
            [FromQuery] string orderBy = default,
            CancellationToken ct = default)
        {
            return await _userGroupChangesService.GetPagedListAsync(changerUserId, groupId, minCreateDate,
                maxCreateDate, offset, limit, sortBy, orderBy, ct).ConfigureAwait(false);
        }
    }
}