using System;
using System.Threading;
using System.Threading.Tasks;
using Identity.Identities.Models;

namespace Identity.Identities.Services
{
    public interface IIdentityTokensService
    {
        Task<IdentityToken> GetAsync(Guid identityId, string value, CancellationToken ct);

        Task<IdentityToken> GetByValueAsync(IdentityTokenType type, string value, CancellationToken ct);

        Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct);

        Task SetIsUsedAsync(Guid id, CancellationToken ct);
    }
}