using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Helpers;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.RequestParameters;
using Crm.Apps.Accounts.Storages;
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

        public Task<Account[]> GetPagedListAsync(AccountGetPagedListRequest request, CancellationToken ct)
        {
            return _accountsStorage.Accounts
                .Where(x =>
                    (!request.IsLocked.HasValue || x.IsLocked == request.IsLocked) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToArrayAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, AccountCreateRequest request, CancellationToken ct)
        {
            var account = new Account();
            var change = account.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.Type = request.Type;
                x.IsLocked = request.IsLocked;
                x.IsDeleted = request.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.Settings = request.Settings?
                    .Select(s => new AccountSetting
                    {
                        AccountId = x.Id,
                        Type = s.Type,
                        Value = s.Value
                    }).ToList();
            });

            var entry = await _accountsStorage.AddAsync(account, ct);
            await _accountsStorage.AddAsync(change, ct);
            await _accountsStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Account account, AccountUpdateRequest request, CancellationToken ct)
        {
            var change = account.UpdateWithLog(userId, x =>
            {
                x.Type = request.Type;
                x.IsLocked = request.IsLocked;
                x.IsDeleted = request.IsDeleted;
                x.Settings = request.Settings
                    .Select(s => new AccountSetting
                    {
                        AccountId = x.Id,
                        Type = s.Type,
                        Value = s.Value
                    })
                    .ToList();
            });

            _accountsStorage.Update(account);
            await _accountsStorage.AddAsync(change, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _accountsStorage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.UpdateWithLog(userId, x => x.IsLocked = true)), ct);

            await _accountsStorage.AddRangeAsync(changes, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _accountsStorage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.UpdateWithLog(userId, x => x.IsLocked = false)), ct);

            await _accountsStorage.AddRangeAsync(changes, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _accountsStorage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.UpdateWithLog(userId, x => x.IsDeleted = true)), ct);

            await _accountsStorage.AddRangeAsync(changes, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _accountsStorage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.UpdateWithLog(userId, x => x.IsDeleted = false)), ct);

            await _accountsStorage.AddRangeAsync(changes, ct);
            await _accountsStorage.SaveChangesAsync(ct);
        }
    }
}