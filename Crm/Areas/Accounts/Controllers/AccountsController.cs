using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Areas.Accounts.Helpers;
using Crm.Areas.Accounts.Models;
using Crm.Areas.Accounts.Storages;
using Crm.Common.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsV1Controller : ControllerBase
    {
        private readonly UserContext _userContext;
        private readonly AccountsStorage _storage;
        private readonly CancellationToken _ct;

        public AccountsV1Controller(UserContext userContext, AccountsStorage storage)
        {
            _userContext = userContext;
            _storage = storage;
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

            var account = await _storage.Accounts
                .Include(x => x.Settings)
                .FirstOrDefaultAsync(x => x.Id == id, _ct)
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

            return await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(_ct)
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
            return await _storage.Accounts
                .Where(x =>
                    (!isLocked.HasValue || x.IsLocked == isLocked) &&
                    (!isDeleted.HasValue || x.IsDeleted == isDeleted) &&
                    (!minCreateDate.HasValue || x.CreateDateTime >= minCreateDate) &&
                    (!maxCreateDate.HasValue || x.CreateDateTime <= maxCreateDate))
                .Sort(sortBy, orderBy)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(_ct)
                .ConfigureAwait(false);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Guid>> Create()
        {
            var account = new Account().CreateWithLog(_userContext.UserId, x =>
            {
                x.Id = new Guid();
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage
                .AddAsync(account, _ct)
                .ConfigureAwait(false);

            await _storage
                .SaveChangesAsync(_ct)
                .ConfigureAwait(false);

            return Created(nameof(Get), entry.Entity.Id);
        }

        [HttpPost("Update")]
        public async Task<ActionResult<Guid>> Update(Guid id, ICollection<AccountSetting> settings)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var account = await GetByIdAsync(id)
                .ConfigureAwait(false);

            if (account == null)
            {
                return NotFound();
            }

            account.UpdateWithLog(_userContext.UserId, x => x.Settings = settings);

            var entry = await _storage
                .AddAsync(account, _ct)
                .ConfigureAwait(false);

            await _storage
                .SaveChangesAsync(_ct)
                .ConfigureAwait(false);

            return Created(nameof(Get), entry.Entity.Id);
        }

        [HttpPost("Lock")]
        public async Task<ActionResult> Lock(ICollection<Guid> ids)
        {
            if (ids == null || ids.All(x => x == Guid.Empty))
            {
                return BadRequest();
            }

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => a.UpdateWithLog(_userContext.UserId, x => x.IsLocked = true), _ct)
                .ConfigureAwait(false);

            await _storage
                .SaveChangesAsync(_ct)
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

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => a.UpdateWithLog(_userContext.UserId, x => x.IsLocked = false), _ct)
                .ConfigureAwait(false);

            await _storage
                .SaveChangesAsync(_ct)
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

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => a.UpdateWithLog(_userContext.UserId, x => x.IsDeleted = true), _ct)
                .ConfigureAwait(false);

            await _storage
                .SaveChangesAsync(_ct)
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

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => a.UpdateWithLog(_userContext.UserId, x => x.IsDeleted = false), _ct)
                .ConfigureAwait(false);

            await _storage
                .SaveChangesAsync(_ct)
                .ConfigureAwait(false);

            return NoContent();
        }

        [NonAction]
        private Task<Account> GetByIdAsync(Guid id)
        {
            return _storage.Accounts
                .Include(x => x.Settings)
                .FirstOrDefaultAsync(x => x.Id == id, _ct);
        }
    }
}