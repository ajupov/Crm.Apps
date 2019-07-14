using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;
using Crm.Apps.Accounts.Services;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Crm.Infrastructure.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/Accounts/Changes")]
    public class AccountChangesController : DefaultController
    {
        private readonly IAccountChangesService _accountChangesService;

        public AccountChangesController(
            IAccountChangesService accountChangesService)
        {
            _accountChangesService = accountChangesService;
        }

        [HttpPost("GetPagedList")]
        [RequirePrivileged]
        public Task<ActionResult<AccountChange[]>> GetPagedList(
            [Required] AccountChangeGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return Get(_accountChangesService.GetPagedListAsync(parameter, ct));
        }
    }
}