using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.OAuth.Models;
using Crm.Apps.OAuth.Parameters;

namespace Crm.Apps.OAuth.Services
{
    public interface IOAuthClientsService
    {
        Task<OAuthClient> GetAsync(
            int id,
            CancellationToken ct);

        Task<OAuthClient[]> GetListAsync(
            IEnumerable<int> ids,
            CancellationToken ct);

        Task<OAuthClient[]> GetPagedListAsync(
            OAuthClientGetPagedListParameter parameter,
            CancellationToken ct);

        OAuthClient CreateAsync(
            Guid userId,
            OAuthClient client,
            CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            OAuthClient oldClient,
            OAuthClient client,
            CancellationToken ct);

        Task LockAsync(
            Guid userId,
            IEnumerable<int> ids,
            CancellationToken ct);

        Task UnlockAsync(
            Guid userId,
            IEnumerable<int> ids,
            CancellationToken ct);

        Task DeleteAsync(
            Guid userId,
            IEnumerable<int> ids,
            CancellationToken ct);

        Task RestoreAsync(
            Guid userId,
            IEnumerable<int> ids,
            CancellationToken ct);
    }
}