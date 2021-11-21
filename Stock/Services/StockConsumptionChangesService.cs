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
    public class StockConsumptionChangesService : IStockConsumptionChangesService
    {
        private readonly StockStorage _storage;

        public StockConsumptionChangesService(StockStorage storage)
        {
            _storage = storage;
        }

        public async Task<StockConsumptionChangeGetPagedListResponse> GetPagedListAsync(
            StockConsumptionChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.StockConsumptionChanges
                .AsNoTracking()
                .Where(x =>
                    (request.StockConsumptionId.IsEmpty() || x.StockConsumptionId == request.StockConsumptionId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new StockConsumptionChangeGetPagedListResponse
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
