using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.RequestParameters;
using Crm.Apps.Accounts.Services;
using Crm.Common.All.UserContext.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
{
    [ApiController]
    [RequirePrivileged]
    [IgnoreApiDocumentation]
    [Route("Api/Accounts/Changes")]
    public class AccountChangesApiController : ControllerBase
    {
        private readonly IAccountChangesService _accountChangesService;

        public AccountChangesApiController(IAccountChangesService accountChangesService)
        {
            _accountChangesService = accountChangesService;
        }

        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<AccountChange>>> GetPagedList(
            AccountChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return await _accountChangesService.GetPagedListAsync(request, ct);
        }
    }
}