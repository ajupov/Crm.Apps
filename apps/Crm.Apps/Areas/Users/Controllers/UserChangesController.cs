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
    [Route("Api/Users/Changes")]
    public class UserChangesController : ControllerBase
    {
        private readonly IUserChangesService _userChangesService;

        public UserChangesController(IUserChangesService userChangesService)
        {
            _userChangesService = userChangesService;
        }

        [HttpGet("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<UserChange>>> GetPagedList(
            [FromQuery] Guid? changerUserId = default,
            [FromQuery] Guid? userId = default,
            [FromQuery] DateTime? minCreateDate = default,
            [FromQuery] DateTime? maxCreateDate = default,
            [FromQuery] int offset = default,
            [FromQuery] int limit = 10,
            [FromQuery] string sortBy = default,
            [FromQuery] string orderBy = default,
            CancellationToken ct = default)
        {
            return await _userChangesService.GetPagedListAsync(changerUserId, userId, minCreateDate, maxCreateDate,
                offset, limit, sortBy, orderBy, ct).ConfigureAwait(false);
        }
    }
}