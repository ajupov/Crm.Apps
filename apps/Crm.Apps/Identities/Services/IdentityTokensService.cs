using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Identities.Services
{
    public class IdentityTokensService : IIdentityTokensService
    {
        private readonly IdentitiesStorage _storage;

        public IdentityTokensService(IdentitiesStorage storage)
        {
            _storage = storage;
        }

        public Task<IdentityToken> GetAsync(Guid identityId, string value, CancellationToken ct)
        {
            return _storage.IdentityTokens.FirstOrDefaultAsync(x => x.IdentityId == identityId && x.Value == value, ct);
        }

        public async Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct)
        {
            var newToken = new IdentityToken
            {
                Id = Guid.NewGuid(),
                IdentityId = token.IdentityId,
                Value = token.Value,
                CreateDateTime = DateTime.UtcNow,
                ExpirationDateTime = token.ExpirationDateTime,
                UseDateTime = default
            };

            var entry = await _storage.AddAsync(newToken, ct).ConfigureAwait(false);
            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);

            return entry.Entity.Id;
        }

        public async Task SetIsUsedAsync(Guid id, CancellationToken ct)
        {
            await _storage.IdentityTokens.Where(x => x.Id == id).ForEachAsync(x => x.UseDateTime = DateTime.UtcNow, ct);

            await _storage.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}