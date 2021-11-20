using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;

namespace Crm.Apps.Stock.Services
{
    public interface IStockConsumptionsService
    {
        Task<StockConsumption> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<StockConsumption>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<StockConsumptionGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            StockConsumptionGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, StockConsumption consumption, CancellationToken ct);

        Task UpdateAsync(Guid userId, StockConsumption oldConsumption, StockConsumption newConsumption, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
