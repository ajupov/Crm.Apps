using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Base.Areas.Accounts.Helpers;
using Crm.Apps.Base.Areas.Accounts.Models;
using Crm.Apps.Base.Areas.Accounts.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Base.Areas.Accounts.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly AccountsStorage _storage;

        public AccountsService(AccountsStorage storage)
        {
            _storage = storage;
        }

        public Task<Account> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return _storage.Accounts.Include(x => x.Settings).FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Account>> GetListAsync(ICollection<Guid> ids, CancellationToken ct)
        {
            return _storage.Accounts.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<Account>> GetPagedListAsync(bool? isLocked, bool? isDeleted, DateTime? minCreateDate,
            DateTime? maxCreateDate, int offset, int limit, string sortBy, string orderBy, CancellationToken ct)
        {
            return _storage.Accounts.Where(x =>
                    (!isLocked.HasValue || x.IsLocked == isLocked) &&
                    (!isDeleted.HasValue || x.IsDeleted == isDeleted) &&
                    (!minCreateDate.HasValue || x.CreateDateTime >= minCreateDate) &&
                    (!maxCreateDate.HasValue || x.CreateDateTime <= maxCreateDate))
                .Sort(sortBy, orderBy)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, CancellationToken ct)
        {
            var account = new Account().CreateWithLog(userId, x =>
            {
                x.Id = new Guid();
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(account, ct).ConfigureAwait(false);

            try
            {
                await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Guid userId, Account oldAccount, Account newAccount, CancellationToken ct)
        {
            oldAccount.UpdateWithLog(userId, x =>
            {
                x.Settings = newAccount.Settings;
                x.IsLocked = newAccount.IsLocked;
                x.IsDeleted = newAccount.IsDeleted;
            });

            _storage.Update(oldAccount);

            return _storage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct)
        {
            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => a.UpdateWithLog(userId, x => x.IsLocked = true), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task UnlockAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct)
        {
            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => a.UpdateWithLog(userId, x => x.IsLocked = false), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct)
        {
            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => a.UpdateWithLog(userId, x => x.IsDeleted = true), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, ICollection<Guid> ids, CancellationToken ct)
        {
            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => a.UpdateWithLog(userId, x => x.IsDeleted = true), ct).ConfigureAwait(false);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}