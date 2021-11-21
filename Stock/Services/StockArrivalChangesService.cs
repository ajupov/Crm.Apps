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
    public class StockArrivalChangesService : IStockArrivalChangesService
    {
        private readonly StockStorage _storage;

        public StockArrivalChangesService(StockStorage storage)
        {
            _storage = storage;
        }

        public async Task<StockArrivalChangeGetPagedListResponse> GetPagedListAsync(
            StockArrivalChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.StockArrivalChanges
                .AsNoTracking()
                .Where(x =>
                    (request.StockArrivalId.IsEmpty() || x.StockArrivalId == request.StockArrivalId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new StockArrivalChangeGetPagedListResponse
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
