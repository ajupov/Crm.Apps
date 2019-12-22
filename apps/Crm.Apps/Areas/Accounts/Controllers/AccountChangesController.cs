using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.RequestParameters;
using Crm.Apps.Areas.Accounts.Services;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
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