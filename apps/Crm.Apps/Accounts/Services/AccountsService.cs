using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;
using Crm.Apps.Accounts.Storages;
using Crm.Utils.ChangesSerializing;
using Crm.Utils.Sorting;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Accounts.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly AccountsStorage _accountsStorage;

        public AccountsService(AccountsStorage accountsStorage)
        {
            _accountsStorage = accountsStorage;
        }

        public Task<Account> GetAsync(Guid id, CancellationToken ct)
        {
            return _accountsStorage.Accounts
                .Include(x => x.Settings)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<Account[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _accountsStorage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ToArrayAsync(ct);
        }

        public Task<Account[]> GetPagedListAsync(AccountGetPagedListParameter parameter, CancellationToken ct)
        {
            return _accountsStorage.Accounts.Where(x =>
                    (!parameter.IsLocked.HasValue || x.IsLocked == parameter.IsLocked) &&
                    (!parameter.IsDeleted.HasValue || x.IsDeleted == parameter.IsDeleted) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, Account account, CancellationToken ct)
        {
            var newAccount = new Account(account.Type);

            newAccount.Settings = account.Settings
                .Select(x => new AccountSetting(newAccount.Id, x.Type, x.Value))
                .ToArray();

            var (oldValue, newValue) = newAccount.CreateWithAudit();

            var change = new AccountChange(newAccount.Id, userId, oldValue, newValue);

            var entry = await _accountsStorage.AddAsync(newAccount, ct);
            await _accountsStorage.AddAsync(change, ct);
            await _accountsStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Account oldAccount, Account account, CancellationToken ct)
        {
            var (oldValue, newValue) = oldAccount.UpdateWithAudit(x =>
            {
                x.IsLocked = account.IsLocked;
                x.IsDeleted = account.IsDeleted;
                x.Settings = account.Settings
                    .Select(s => new AccountSetting(x.Id, s.Type, s.Value))
                    .ToArray();
            });

            var change = new AccountChange(oldAccount.Id, userId, oldValue, newValue);

            _accountsStorage.Update(oldAccount);
            await _accountsStorage.AddAsync(change, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _accountsStorage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a =>
                {
                    var (oldValue, newValue) = a.UpdateWithAudit(x => x.IsLocked = true);

                    changes.Add(new AccountChange(a.Id, userId, oldValue, newValue));
                }, ct);

            await _accountsStorage.AddRangeAsync(changes, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _accountsStorage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a =>
                {
                    var (oldValue, newValue) = a.UpdateWithAudit(x => x.IsLocked = false);

                    changes.Add(new AccountChange(a.Id, userId, oldValue, newValue));
                }, ct);

            await _accountsStorage.AddRangeAsync(changes, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _accountsStorage.Accounts.Where(x => ids.Contains(x.Id))
                .ForEachAsync(a =>
                {
                    var (oldValue, newValue) = a.UpdateWithAudit(x => x.IsDeleted = true);

                    changes.Add(new AccountChange(a.Id, userId, oldValue, newValue));
                }, ct);

            await _accountsStorage.AddRangeAsync(changes, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _accountsStorage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a =>
                {
                    var (oldValue, newValue) = a.UpdateWithAudit(x => x.IsDeleted = false);

                    changes.Add(new AccountChange(a.Id, userId, oldValue, newValue));
                }, ct);

            await _accountsStorage.AddRangeAsync(changes, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }
    }
}