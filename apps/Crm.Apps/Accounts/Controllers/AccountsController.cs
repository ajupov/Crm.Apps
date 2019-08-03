using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;
using Crm.Apps.Accounts.Services;
using Crm.Common.UserContext;
using Crm.Common.UserContext.Attributes;
using Crm.Infrastructure.ApiDocumentation.Attributes;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/Accounts")]
    public class AccountsApiController : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IAccountsService _accountsService;

        public AccountsApiController(
            IUserContext userContext,
            IAccountsService accountsService)
        {
            _userContext = userContext;
            _accountsService = accountsService;
        }

        [HttpGet("GetTypes")]
        [RequirePrivileged]
        public ActionResult<Dictionary<string, AccountType>> GetTypes()
        {
            return EnumsExtensions.GetAsDictionary<AccountType>();
        }

        [HttpGet("Get")]
        [RequirePrivileged]
        public async Task<ActionResult<Account>> Get([Required] Guid id, CancellationToken ct = default)
        {
            var model = await _accountsService.GetAsync(id, ct);
            if (model == null)
            {
                return BadRequest(id);
            }

            return model;
        }

        [HttpPost("GetList")]
        [RequirePrivileged]
        public async Task<ActionResult<Account[]>> GetList([Required] List<Guid> ids, CancellationToken ct = default)
        {
            return await _accountsService.GetListAsync(ids, ct);
        }

        [HttpPost("GetPagedList")]
        [RequirePrivileged]
        public async Task<ActionResult<Account[]>> GetPagedList(
            AccountGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return await _accountsService.GetPagedListAsync(parameter, ct);
        }

        [HttpPost("Create")]
        [RequirePrivileged]
        public async Task<ActionResult<Guid>> Create(Account account, CancellationToken ct = default)
        {
            var id = await _accountsService.CreateAsync(_userContext.UserId, account, ct);

            return Created("Get", id);
        }

        [HttpPost("Update")]
        [RequirePrivileged]
        public async Task<ActionResult> Update(Account account, CancellationToken ct = default)
        {
            var oldAccount = await _accountsService.GetAsync(account.Id, ct);
            if (oldAccount == null)
            {
                return BadRequest(account);
            }

            await _accountsService.UpdateAsync(_userContext.UserId, oldAccount, account, ct);

            return NoContent();
        }

        [HttpPost("Lock")]
        [RequirePrivileged]
        public async Task<ActionResult> Lock([Required] List<Guid> ids, CancellationToken ct = default)
        {
            await _accountsService.LockAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [HttpPost("Unlock")]
        [RequirePrivileged]
        public async Task<ActionResult> Unlock([Required] List<Guid> ids, CancellationToken ct = default)
        {
            await _accountsService.UnlockAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [HttpPost("Delete")]
        [RequirePrivileged]
        public async Task<ActionResult> Delete([Required] List<Guid> ids, CancellationToken ct = default)
        {
            await _accountsService.DeleteAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }

        [HttpPost("Restore")]
        [RequirePrivileged]
        public async Task<ActionResult> Restore([Required] List<Guid> ids, CancellationToken ct = default)
        {
            await _accountsService.RestoreAsync(_userContext.UserId, ids, ct);

            return NoContent();
        }
    }
}