using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;

namespace Crm.Apps.Stock.Services
{
    public interface IStockArrivalsService
    {
        Task<StockArrival> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<StockArrival>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<StockArrivalGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            StockArrivalGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, StockArrival arrival, CancellationToken ct);

        Task UpdateAsync(Guid userId, StockArrival oldArrival, StockArrival newArrival, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
