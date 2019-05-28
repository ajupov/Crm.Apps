using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;

namespace Crm.Clients.Identities.Clients
{
    public interface IIdentityTokensClient
    {
        Task<IdentityToken> GetAsync(Guid identityId, string value, CancellationToken ct = default);
        
        Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct = default);
        
        Task SetIsUsedAsync(Guid id, CancellationToken ct = default);
    }
}