using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;

namespace Crm.Apps.Clients.Deals.Clients
{
    public interface IDealStatusesClient
    {
        Task<DealStatus> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<DealStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<DealStatus>> GetPagedListAsync(
            DealStatusGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(DealStatus status, CancellationToken ct = default);

        Task UpdateAsync(DealStatus status, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}