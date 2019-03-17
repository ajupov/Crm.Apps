using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Areas.Accounts.Models;
using Crm.Areas.Accounts.Services;
using Crm.Common.Types;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsV1Controller : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly IAccountsService _accountsService;
        private readonly CancellationToken _ct;

        public AccountsV1Controller(
            UserContext userContext,
            IAccountsService accountsService)
        {
            _userContext = userContext;
            _accountsService = accountsService;
            _ct = CancellationTokenSource.CreateLinkedTokenSource(HttpContext.RequestAborted).Token;
        }

        [HttpGet("")]
        public ActionResult Status()
        {
            return Ok();
        }

        [HttpGet("Get")]
        public async Task<ActionResult<Account>> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var account = await _accountsService.GetByIdAsync(id, _ct)
                .ConfigureAwait(false);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpGet("GetList")]
        public async Task<ActionResult<ICollection<Account>>> GetList(ICollection<Guid> ids)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            return await _accountsService.GetListAsync(ids, _ct)
                .ConfigureAwait(false);
        }

        [HttpGet("GetPagedList")]
        public async Task<ActionResult<ICollection<Account>>> GetPagedList(
            bool? isLocked = default,
            bool? isDeleted = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = default,
            string orderBy = default)
        {
            return await _accountsService.GetPagedListAsync(
                    isLocked,
                    isDeleted,
                    minCreateDate,
                    maxCreateDate,
                    offset,
                    limit,
                    sortBy,
                    orderBy,
                    _ct)
                .ConfigureAwait(false);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create()
        {
            var id = await _accountsService.CreateAsync(_userContext.UserId, _ct)
                .ConfigureAwait(false);

            return Created(nameof(Get), id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult<Guid>> Update(Guid id, ICollection<AccountSetting> settings)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var account = await _accountsService.GetByIdAsync(id, _ct)
                .ConfigureAwait(false);

            if (account == null)
            {
                return NotFound();
            }

            await _accountsService.UpdateAsync(_userContext.UserId, account, settings, _ct)
                .ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Lock")]
        public async Task<ActionResult> Lock(ICollection<Guid> ids)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _accountsService.LockAsync(_userContext.UserId, ids, _ct)
                .ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Unlock")]
        public async Task<ActionResult> Unlock(ICollection<Guid> ids)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _accountsService.UnlockAsync(_userContext.UserId, ids, _ct)
                .ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(ICollection<Guid> ids)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _accountsService.DeleteAsync(_userContext.UserId, ids, _ct)
                .ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore(ICollection<Guid> ids)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _accountsService.RestoreAsync(_userContext.UserId, ids, _ct)
                .ConfigureAwait(false);

            return NoContent();
        }
    }
}