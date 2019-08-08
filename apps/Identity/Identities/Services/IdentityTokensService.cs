using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Identities.Models;
using Identity.Identities.Storages;
using Microsoft.EntityFrameworkCore;

namespace Identity.Identities.Services
{
    public class IdentityTokensService : IIdentityTokensService
    {
        private readonly IdentitiesStorage _identitiesStorage;

        public IdentityTokensService(IdentitiesStorage identitiesStorage)
        {
            _identitiesStorage = identitiesStorage;
        }

        public Task<IdentityToken> GetAsync(Guid identityId, string value, CancellationToken ct)
        {
            return _identitiesStorage.IdentityTokens
                .FirstOrDefaultAsync(x => x.IdentityId == identityId && x.Value == value, ct);
        }

        public Task<IdentityToken> GetByValueAsync(IdentityTokenType type, string value, CancellationToken ct)
        {
            return _identitiesStorage.IdentityTokens
                .FirstOrDefaultAsync(x => x.Type == type && x.Value == value, ct);
        }

        public async Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct)
        {
            var newToken = new IdentityToken(token.IdentityId, token.Type, token.Value, token.ExpirationDateTime,
                token.UserAgent, token.IpAddress);

            var entry = await _identitiesStorage.AddAsync(newToken, ct);
            await _identitiesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public async Task SetIsUsedAsync(Guid id, CancellationToken ct)
        {
            await _identitiesStorage.IdentityTokens
                .Where(x => x.Id == id)
                .ForEachAsync(x => x.UseDateTime = DateTime.UtcNow, ct);

            await _identitiesStorage.SaveChangesAsync(ct);
        }
    }
}