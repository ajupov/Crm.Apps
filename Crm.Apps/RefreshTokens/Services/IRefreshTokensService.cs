using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.RefreshTokens.Models;

namespace Crm.Apps.RefreshTokens.Services
{
    public interface IRefreshTokensService
    {
        Task<RefreshToken> GetActiveAsync(string key, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, string key, string value, CancellationToken ct);

        Task SetIsExpiredAsync(string key, CancellationToken ct);
    }
}