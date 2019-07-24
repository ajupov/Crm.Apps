using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;

namespace Crm.Clients.Identities.Clients
{
    public interface IIdentityChangesClient
    {
        Task<IdentityChange[]> GetPagedListAsync(
            Guid identityId = default,
            Guid changerUserId = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc",
            CancellationToken ct = default);
    }
}