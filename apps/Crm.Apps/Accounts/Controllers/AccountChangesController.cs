using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;
using Crm.Apps.Accounts.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
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

        [HttpPost("GetPagedList")]
        [RequireAny(Permission.System, Permission.Development, Permission.Administration, Permission.TechnicalSupport)]
        public async Task<ActionResult<List<AccountChange>>> GetPagedList(AccountChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _accountChangesService.GetPagedListAsync(parameter, ct).ConfigureAwait(false);
        }
    }
}