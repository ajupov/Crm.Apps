using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Accounts.Helpers;
using Crm.Apps.Areas.Accounts.Models;
using Crm.Apps.Areas.Accounts.Parameters;
using Crm.Apps.Areas.Accounts.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Accounts.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly AccountsStorage _storage;

        public AccountsService(AccountsStorage storage)
        {
            _storage = storage;
        }

        public Task<Account> GetAsync(Guid id, CancellationToken ct)
        {
            return _storage.Accounts.Include(x => x.Settings).FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Account>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Accounts.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
        }

        public Task<List<Account>> GetPagedListAsync(AccountGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.Accounts.Where(x =>
                    (!parameter.IsLocked.HasValue || x.IsLocked == parameter.IsLocked) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, Account account, CancellationToken ct)
        {
            var newAccount = new Account();
            var change = newAccount.WithCreateLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.Settings = account.Settings;
                x.IsLocked = account.IsLocked;
                x.IsDeleted = account.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
            });

            var entry = await _storage.AddAsync(newAccount, ct).ConfigureAwait(false);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Account oldAccount, Account newAccount, CancellationToken ct)
        {
            var change = oldAccount.WithUpdateLog(userId, x =>
            {
                x.IsLocked = newAccount.IsLocked;
                x.IsDeleted = newAccount.IsDeleted;
                x.Settings = newAccount.Settings;
            });

            _storage.Update(oldAccount);
            await _storage.AddAsync(change, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.WithUpdateLog(userId, x => x.IsLocked = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.WithUpdateLog(userId, x => x.IsLocked = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.WithUpdateLog(userId, x => x.IsDeleted = true)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.WithUpdateLog(userId, x => x.IsDeleted = false)), ct)
                .ConfigureAwait(false);

            await _storage.AddRangeAsync(changes, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}