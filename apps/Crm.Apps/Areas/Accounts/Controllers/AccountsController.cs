using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.Services;
using Crm.Common.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Apps.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("Api/Accounts")]
    public class AccountsV1Controller : ControllerBase
    {
        private readonly IUserContext _userContext;
        private readonly IAccountsService _accountsService;

        public AccountsV1Controller(IUserContext userContext, IAccountsService accountsService)
        {
            _userContext = userContext;
            _accountsService = accountsService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Account>> Get(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var account = await _accountsService.GetByIdAsync(id, ct).ConfigureAwait(false);
            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpGet("GetList")]
        public async Task<ActionResult<ICollection<Account>>> GetList(ICollection<Guid> ids,
            CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            return await _accountsService.GetListAsync(ids, ct).ConfigureAwait(false);
        }

        [HttpGet("GetPagedList")]
        public async Task<ActionResult<ICollection<Account>>> GetPagedList(bool? isLocked = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            return await _accountsService.GetPagedListAsync(isLocked, isDeleted, minCreateDate, maxCreateDate, offset,
                limit, sortBy, orderBy, ct).ConfigureAwait(false);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create(CancellationToken ct = default)
        {
            var id = await _accountsService.CreateAsync(_userContext.UserId, ct).ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult<Guid>> Update(Account newAccount, CancellationToken ct = default)
        {
            if (newAccount.Id == Guid.Empty)
            {
                return BadRequest();
            }

            var oldAccount = await _accountsService.GetByIdAsync(newAccount.Id, ct).ConfigureAwait(false);
            if (oldAccount == null)
            {
                return NotFound();
            }

            await _accountsService.UpdateAsync(_userContext.UserId, oldAccount, newAccount, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Lock")]
        public async Task<ActionResult> Lock(ICollection<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _accountsService.LockAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Unlock")]
        public async Task<ActionResult> Unlock(ICollection<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _accountsService.UnlockAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(ICollection<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _accountsService.DeleteAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore(ICollection<Guid> ids, CancellationToken ct = default)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _accountsService.RestoreAsync(_userContext.UserId, ids, ct).ConfigureAwait(false);

            return NoContent();
        }
    }
}