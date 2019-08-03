using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;
using Crm.Apps.Accounts.Services;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
{
    [ApiController]
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
        [RequirePrivileged]
        public async Task<ActionResult<AccountChange[]>> GetPagedList(
            AccountChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _accountChangesService.GetPagedListAsync(parameter, ct);
        }
    }
}