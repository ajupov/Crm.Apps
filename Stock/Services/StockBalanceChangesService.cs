using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Stock.Storages;
using Crm.Apps.Stock.V1.Requests;
using Crm.Apps.Stock.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Stock.Services
{
    public class StockBalanceChangesService : IStockBalanceChangesService
    {
        private readonly StockStorage _storage;

        public StockBalanceChangesService(StockStorage storage)
        {
            _storage = storage;
        }

        public async Task<StockBalanceChangeGetPagedListResponse> GetPagedListAsync(
            StockBalanceChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.StockBalanceChanges
                .AsNoTracking()
                .Where(x =>
                    (request.StockBalanceId.IsEmpty() || x.StockBalanceId == request.StockBalanceId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new StockBalanceChangeGetPagedListResponse
            {
                TotalCount = await changes
                    .CountAsync(ct),
                Changes = await changes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }
    }
}
