using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;

namespace Crm.Apps.Clients.Deals.Clients
{
    public interface IDealsClient
    {
        Task<Deal> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Deal>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Deal>> GetPagedListAsync(DealGetPagedListRequestParameter request, CancellationToken ct = default);

        Task<Guid> CreateAsync(Deal deal, CancellationToken ct = default);

        Task UpdateAsync(Deal deal, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}