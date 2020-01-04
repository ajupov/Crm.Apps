using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Deals.Models;

namespace Crm.Clients.Deals.Clients
{
    public interface IDealChangesClient
    {
        Task<List<DealChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? dealId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}