using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;

namespace Crm.Apps.Stock.Services
{
    public interface IStockBalancesService
    {
        Task<StockBalance> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<StockBalance>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<StockBalanceGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            StockBalanceGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, StockBalance balance, CancellationToken ct);

        Task UpdateAsync(Guid userId, StockBalance oldBalance, StockBalance newBalance, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
