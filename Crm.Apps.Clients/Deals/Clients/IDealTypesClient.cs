using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;

namespace Crm.Apps.Clients.Deals.Clients
{
    public interface IDealTypesClient
    {
        Task<DealType> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<DealType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<DealType>> GetPagedListAsync(
            DealTypeGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(DealType type, CancellationToken ct = default);

        Task UpdateAsync(DealType type, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}