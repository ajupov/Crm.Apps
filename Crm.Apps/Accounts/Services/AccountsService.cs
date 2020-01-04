using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Accounts.Helpers;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.RequestParameters;
using Crm.Apps.Accounts.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Accounts.Services
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
            return _storage.Accounts
                .AsNoTracking()
                .Include(x => x.Settings)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Account>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _storage.Accounts
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<Account>> GetPagedListAsync(AccountGetPagedListRequestParameter request, CancellationToken ct)
        {
            return _storage.Accounts
                .AsNoTracking()
                .Where(x =>
                    (!request.IsLocked.HasValue || x.IsLocked == request.IsLocked) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, Account account, CancellationToken ct)
        {
            var newAccount = new Account();
            var change = newAccount.CreateWithLog(userId, x =>
            {
                x.Id = Guid.NewGuid();
                x.Type = account.Type;
                x.IsLocked = account.IsLocked;
                x.IsDeleted = account.IsDeleted;
                x.CreateDateTime = DateTime.UtcNow;
                x.Settings = account.Settings?
                    .Select(s => new AccountSetting
                    {
                        AccountId = x.Id,
                        Type = s.Type,
                        Value = s.Value
                    }).ToList();
            });

            var entry = await _storage.AddAsync(newAccount, ct);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task UpdateAsync(Guid userId, Account oldAccount, Account newAccount, CancellationToken ct)
        {
            var change = oldAccount.UpdateWithLog(userId, x =>
            {
                x.Type = newAccount.Type;
                x.IsLocked = newAccount.IsLocked;
                x.IsDeleted = newAccount.IsDeleted;
                x.Settings = newAccount.Settings
                    .Select(s => new AccountSetting
                    {
                        AccountId = x.Id,
                        Type = s.Type,
                        Value = s.Value
                    })
                    .ToList();
            });

            _storage.Update(oldAccount);
            await _storage.AddAsync(change, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.UpdateWithLog(userId, x => x.IsLocked = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.UpdateWithLog(userId, x => x.IsLocked = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.UpdateWithLog(userId, x => x.IsDeleted = true)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct)
        {
            var changes = new List<AccountChange>();

            await _storage.Accounts
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(a => changes.Add(a.UpdateWithLog(userId, x => x.IsDeleted = false)), ct);

            await _storage.AddRangeAsync(changes, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}