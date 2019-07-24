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
using Crm.Infrastructure.Mvc;
using Crm.Utils.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Accounts.Controllers
{
    [ApiController]
    [IgnoreApiDocumentation]
    [Route("Api/Accounts")]
    public class AccountsApiController : DefaultApiController
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
        public ActionResult<Dictionary<AccountType, string>> GetTypes()
        {
            return Get(EnumsExtensions.GetAsDictionary<AccountType>());
        }

        [HttpGet("Get")]
        [RequirePrivileged]
        public Task<ActionResult<Account>> Get(
            [Required] Guid id,
            CancellationToken ct = default)
        {
            return Get(_accountsService.GetAsync(id, ct));
        }

        [HttpPost("GetList")]
        [RequirePrivileged]
        public Task<ActionResult<Account[]>> GetList(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            return Get(_accountsService.GetListAsync(ids, ct));
        }

        [HttpPost("GetPagedList")]
        [RequirePrivileged]
        public Task<ActionResult<Account[]>> GetPagedList(
            AccountGetPagedListParameter parameter,
            CancellationToken ct = default)
        {
            return Get(_accountsService.GetPagedListAsync(parameter, ct));
        }

        [HttpPost("Create")]
        [RequirePrivileged]
        public Task<ActionResult<Guid>> Create(
            [Required] Account account,
            CancellationToken ct = default)
        {
            return Create(_accountsService.CreateAsync(_userContext.UserId, account, ct));
        }

        [HttpPost("Update")]
        [RequirePrivileged]
        public async Task<ActionResult> Update(
            [Required] Account account,
            CancellationToken ct = default)
        {
            var oldAccount = await _accountsService.GetAsync(account.Id, ct);

            return await Action(_accountsService.UpdateAsync(_userContext.UserId, oldAccount, account, ct), oldAccount);
        }

        [HttpPost("Lock")]
        [RequirePrivileged]
        public Task<ActionResult> Lock(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            return Action(_accountsService.LockAsync(_userContext.UserId, ids, ct));
        }

        [HttpPost("Unlock")]
        [RequirePrivileged]
        public Task<ActionResult> Unlock(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            return Action(_accountsService.UnlockAsync(_userContext.UserId, ids, ct));
        }

        [HttpPost("Delete")]
        [RequirePrivileged]
        public Task<ActionResult> Delete(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            return Action(_accountsService.DeleteAsync(_userContext.UserId, ids, ct));
        }

        [HttpPost("Restore")]
        [RequirePrivileged]
        public Task<ActionResult> Restore(
            [Required] List<Guid> ids,
            CancellationToken ct = default)
        {
            return Action(_accountsService.RestoreAsync(_userContext.UserId, ids, ct));
        }
    }
}