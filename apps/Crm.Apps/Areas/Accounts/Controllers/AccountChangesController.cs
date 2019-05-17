using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("Api/Accounts/Changes")]
    public class AccountChangesController : ControllerBase
    {
        private readonly IAccountChangesService _accountChangesService;

        public AccountChangesController(IAccountChangesService accountChangesService)
        {
            _accountChangesService = accountChangesService;
        }

        [HttpGet("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<AccountChange>>> GetPagedList(
            [FromQuery] Guid? changerUserId = default,
            [FromQuery] Guid? accountId = default,
            [FromQuery] DateTime? minCreateDate = default,
            [FromQuery] DateTime? maxCreateDate = default,
            [FromQuery] int offset = default,
            [FromQuery] int limit = 10,
            [FromQuery] string sortBy = default,
            [FromQuery] string orderBy = default,
            CancellationToken ct = default)
        {
            return await _accountChangesService.GetPagedListAsync(changerUserId, accountId, minCreateDate,
                maxCreateDate, offset, limit, sortBy, orderBy, ct).ConfigureAwait(false);
        }
    }
}