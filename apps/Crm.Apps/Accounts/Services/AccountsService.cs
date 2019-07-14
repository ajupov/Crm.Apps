using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Helpers;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;
using Crm.Apps.Accounts.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Accounts.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly AccountsStorage _storage;

        public AccountsService(
            AccountsStorage storage)
        {
            _storage = storage;
        }

        public Task<Account> GetAsync(
            Guid id,
            CancellationToken ct)
        {
            return _storage.Accounts
                .Include(x => x.Settings)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<Account[]> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct)
        {
            return _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<Account[]> GetPagedListAsync(
            AccountGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.Accounts.Where(x =>
                    (!parameter.IsLocked.HasValue || x.IsLocked == parameter.IsLocked) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(
            Guid userId,
            Account account,
            CancellationToken ct)
        {
            var newAccount = new Account(account.Type);

            newAccount.Settings = account.Settings
                .Select(s => new AccountSetting(newAccount.Id, s.Type, s.Value))
                .ToList();

            var (oldValue, newValue) = newAccount.CreateWithAudit();

            var change = new AccountChange(newAccount.Id, userId, oldValue, newValue);

            var entry = await _storage.AddAsync(newAccount, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(
            Guid userId,
            Account oldAccount,
            Account newAccount,
            CancellationToken ct)
        {
            var (oldValue, newValue) = oldAccount.UpdateWithAudit(x =>
            {
                x.IsLocked = newAccount.IsLocked;
                x.IsDeleted = newAccount.IsDeleted;
                x.Settings = newAccount.Settings
                    .Select(s => new AccountSetting(x.Id, s.Type, s.Value))
                    .ToList();
            });

            var change = new AccountChange(oldAccount.Id, userId, oldValue, newValue);

            _storage.Update(oldAccount);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(
            Guid userId,
            IEnumerable<Guid> ids,
            CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a =>
                {
                    var (oldValue, newValue) = a.UpdateWithAudit(x => x.IsLocked = true);

                    changes.Add(new AccountChange(a.Id, userId, oldValue, newValue));
                }, ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(
            Guid userId,
            IEnumerable<Guid> ids,
            CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a =>
                {
                    var (oldValue, newValue) = a.UpdateWithAudit(x => x.IsLocked = false);

                    changes.Add(new AccountChange(a.Id, userId, oldValue, newValue));
                }, ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(
            Guid userId,
            IEnumerable<Guid> ids,
            CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a =>
                {
                    var (oldValue, newValue) = a.UpdateWithAudit(x => x.IsDeleted = true);

                    changes.Add(new AccountChange(a.Id, userId, oldValue, newValue));
                }, ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(
            Guid userId,
            IEnumerable<Guid> ids,
            CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a =>
                {
                    var (oldValue, newValue) = a.UpdateWithAudit(x => x.IsDeleted = false);

                    changes.Add(new AccountChange(a.Id, userId, oldValue, newValue));
                }, ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}