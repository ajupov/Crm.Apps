using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Areas.Accounts.Helpers;
using Crm.Areas.Accounts.Models;
using Crm.Areas.Accounts.Storages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crm.Areas.Accounts.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountsV1Controller : ControllerBase
    {
        private readonly AccountsStorage _storage;
        private readonly CancellationToken _ct;

        public AccountsV1Controller(AccountsStorage storage)
        {
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
        public async Task<ActionResult<ICollection<Account>>> GetList(IEnumerable<Guid> ids)
        {
            if (ids == null || !ids.Any(x => x != Guid.Empty))
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
            var changerUserId = new Guid();

            var account = new Account
            {
                Id = new Guid(),
                CreateDateTime = DateTime.UtcNow
            };

            account.LogCreating(changerUserId);

            var entry = await _storage
                .AddAsync(account, _ct)
                .ConfigureAwait(false);

            await _storage
                .SaveChangesAsync(_ct)
                .ConfigureAwait(false);

            return CreatedAtAction(nameof(Account), entry.Entity.Id);
        }

        [HttpPost("Lock")]
        public async Task<ActionResult> Lock(ICollection<Guid> ids)
        {
            if (ids == null || !ids.Any(x => x != Guid.Empty))
            {
                return BadRequest();
            }

            var changerUserId = new Guid();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    var oldValue = x.IsLocked;

                    x.IsLocked = true;

                    x.LogUpdating(changerUserId, nameof(x.IsLocked), oldValue, x.IsLocked);
                }, _ct);

            await _storage
                .SaveChangesAsync(_ct)
                .ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Unlock")]
        public async Task<ActionResult> Unlock(ICollection<Guid> ids)
        {
            if (ids == null || !ids.Any(x => x != Guid.Empty))
            {
                return BadRequest();
            }

            var changerUserId = new Guid();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    var oldValue = x.IsLocked;

                    x.IsLocked = false;

                    x.LogUpdating(changerUserId, nameof(x.IsLocked), oldValue, x.IsLocked);
                }, _ct);

            await _storage
                .SaveChangesAsync(_ct)
                .ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Delete")]
        public async Task<ActionResult> Delete(ICollection<Guid> ids)
        {
            if (ids == null || !ids.Any(x => x != Guid.Empty))
            {
                return BadRequest();
            }

            var changerUserId = new Guid();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    var oldValue = x.IsDeleted;

                    x.IsDeleted = true;

                    x.LogUpdating(changerUserId, nameof(x.IsDeleted), oldValue, x.IsDeleted);
                }, _ct);

            await _storage
                .SaveChangesAsync(_ct)
                .ConfigureAwait(false);

            return NoContent();
        }

        [HttpPost("Restore")]
        public async Task<ActionResult> Restore(ICollection<Guid> ids)
        {
            if (ids == null || !ids.Any(x => x != Guid.Empty))
            {
                return BadRequest();
            }

            var changerUserId = new Guid();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    var oldValue = x.IsDeleted;

                    x.IsDeleted = false;

                    x.LogUpdating(changerUserId, nameof(x.IsDeleted), oldValue, x.IsDeleted);
                }, _ct);

            await _storage
                .SaveChangesAsync(_ct)
                .ConfigureAwait(false);

            return NoContent();
        }
    }
}