using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Enums;
using Crm.Apps.Account.Models;
using Crm.Apps.Account.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Account.Services
{
    public class AccountFlagsService : IAccountFlagsService
    {
        private readonly AccountStorage _storage;

        public AccountFlagsService(AccountStorage storage)
        {
            _storage = storage;
        }

        public Task<bool> IsSetAsync(Guid accountId, AccountFlagType type, CancellationToken ct)
        {
            return _storage.AccountFlags
                .AsNoTracking()
                .AnyAsync(x => x.AccountId == accountId && x.Type == type, cancellationToken: ct);
        }

        public async Task<List<AccountFlagType>> GetNotSetListAsync(Guid accountId, CancellationToken ct)
        {
            var allFlags = EnumsExtensions.GetValues<AccountFlagType>();

            var setFlags = await _storage.AccountFlags
                .AsNoTracking()
                .Where(x => x.AccountId == accountId)
                .Select(x => x.Type)
                .ToListAsync(ct);

            return allFlags
                .Except(setFlags)
                .ToList();
        }

        public async Task SetAsync(Guid accountId, AccountFlagType type, CancellationToken ct)
        {
            var flag = await _storage.AccountFlags
                .FirstOrDefaultAsync(x => x.Type == type && x.AccountId == accountId, ct);
            if (flag != null)
            {
                flag.SetDateTime = DateTime.UtcNow;

                _storage.Update(flag);
            }
            else
            {
                flag = new AccountFlag
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    Type = type,
                    SetDateTime = DateTime.UtcNow
                };

                await _storage.AddAsync(flag, ct);
            }

            await _storage.SaveChangesAsync(ct);
        }
    }
}
