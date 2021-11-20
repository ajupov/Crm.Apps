using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Stock.Models;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;

namespace Crm.Apps.Stock.Services
{
    public interface IStockRoomsService
    {
        Task<StockRoom> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<StockRoom>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<StockRoomGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            StockRoomGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, StockRoom room, CancellationToken ct);

        Task UpdateAsync(Guid userId, StockRoom oldRoom, StockRoom newRoom, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
