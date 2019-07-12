using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;

namespace Crm.Clients.Activities.Clients
{
    public interface IActivityAttributeChangesClient
    {
        Task<List<ActivityAttributeChange>> GetPagedListAsync(Guid? changerUserId = default,
            Guid? attributeId = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);
    }
}