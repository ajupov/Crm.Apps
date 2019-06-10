using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;

namespace Crm.Clients.Identities.Clients
{
    public interface IIdentityChangesClient
    {
        Task<List<IdentityChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? identityId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}