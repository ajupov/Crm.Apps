using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.Parameters;
using Crm.Apps.Areas.Accounts.Services;
using Crm.Apps.Utils;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/Accounts")]
    public class AccountsApiController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IAccountsService _accountsService;

        public AccountsApiController(IUserContext userContext, IAccountsService accountsService)
        {
            _userContext = userContext;
            _accountsService = accountsService;
        }

        [RequirePrivileged]
        [HttpGet("GetTypes")]
        public ActionResult<Dictionary<string, AccountType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AccountType>();
        }

        [RequirePrivileged]
        [HttpGet("Get")]
        public async Task<ActionResult<Account>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var account = await _accountsService.GetAsync(id, ct);
            if (account == null)
            {
                return NotFound(id);
            }

            return account;
        }

        [RequirePrivileged]
        [HttpPost("GetList")]
        public async Task<ActionResult<List<Account>>> GetList(
            [Required] IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return await _accountsService.GetListAsync(ids, ct);
        }

        [RequirePrivileged]
        [HttpPost("GetPagedList")]
        public async Task<ActionResult<List<Account>>> GetPagedList(
            AccountGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _accountsService.GetPagedListAsync(parameter, ct);
        }

        [RequirePrivileged]
        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(Account account, CancellationToken ct = default)
        {
            var id = await _accountsService.CreateAsync(_userContext.UserId, account, ct);

            return Created("Get", id);
        }

        [RequirePrivileged]
        [HttpPost("Update")]
        public async Task<ActionResult> Update(Account account, CancellationToken ct = default)
        {
            var oldAccount = await _accountsService.GetAsync(account.Id, ct);
            if (oldAccount == null)
            {
                return NotFound(account.Id);
            }

            await _accountsService.UpdateAsync(_userContext.UserId, oldAccount, account, ct);

            return NoContent();
        }

        [RequirePrivileged]
        [HttpPost("Lock")]
        public async Task<ActionResult> Lock([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            await _accountsService.LockAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [RequirePrivileged]
        [HttpPost("Unlock")]
        public async Task<ActionResult> Unlock([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            await _accountsService.UnlockAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [RequirePrivileged]
        [HttpPost("Delete")]
        public async Task<ActionResult> Delete([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            await _accountsService.DeleteAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [RequirePrivileged]
        [HttpPost("Restore")]
        public async Task<ActionResult> Restore([Required] IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            await _accountsService.RestoreAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }
    }
}