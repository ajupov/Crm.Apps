using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Flags.Models;
using Crm.Apps.Flags.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Flags.Services
{
    public class AccountFlagsService : IAccountFlagsService
    {
        private readonly FlagsStorage _storage;

        public AccountFlagsService(FlagsStorage storage)
        {
            _storage = storage;
        }

        public Task<bool> IsSetAsync(Guid accountId, AccountFlagType type, CancellationToken ct)
        {
            return _storage.AccountFlags
                .AsNoTracking()
                .AnyAsync(x => x.AccountId == accountId && x.Type == type, cancellationToken: ct);
        }

        public async Task SetAsync(Guid accountId, AccountFlagType type, CancellationToken ct)
        {
            var flag = new AccountFlag
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                Type = type,
                SetDateTime = DateTime.UtcNow
            };

            await _storage.AddAsync(flag, ct);
            await _storage.SaveChangesAsync(ct);
        }
    }
}
