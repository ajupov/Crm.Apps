using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.RefreshTokens.Models;
using Crm.Apps.RefreshTokens.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.RefreshTokens.Services
{
    public class RefreshTokensService : IRefreshTokensService
    {
        private readonly RefreshTokensStorage _storage;

        public RefreshTokensService(RefreshTokensStorage storage)
        {
            _storage = storage;
        }

        public Task<RefreshToken> GetActiveAsync(string key, CancellationToken ct)
        {
            return _storage.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Key == key && x.ExpirationDateTime > DateTime.UtcNow, ct);
        }

        public async Task<Guid> CreateAsync(Guid userId, string key, string value, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Key = key,
                Value = value,
                CreateDateTime = now,
                ExpirationDateTime = now.AddYears(1)
            };

            var entry = await _storage.AddAsync(newRefreshToken, ct);
            await _storage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task SetIsExpiredAsync(string key, CancellationToken ct)
        {
            await _storage.RefreshTokens
                .Where(x => x.Key == key)
                .ForEachAsync(x => x.ExpirationDateTime = DateTime.UtcNow, ct);

            await _storage.SaveChangesAsync(ct);
        }
    }
}