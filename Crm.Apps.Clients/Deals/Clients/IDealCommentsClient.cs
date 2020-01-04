using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;

namespace Crm.Apps.Clients.Deals.Clients
{
    public interface IDealCommentsClient
    {
        Task<List<DealComment>> GetPagedListAsync(Guid? dealId = default, Guid? commentatorUserId = default,
            string value = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task CreateAsync(DealComment comment, CancellationToken ct = default);
    }
}