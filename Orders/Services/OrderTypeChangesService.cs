using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Orders.Storages;
using Crm.Apps.Orders.V1.Requests;
using Crm.Apps.Orders.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Orders.Services
{
    public class OrderTypeChangesService : IOrderTypeChangesService
    {
        private readonly OrdersStorage _storage;

        public OrderTypeChangesService(OrdersStorage storage)
        {
            _storage = storage;
        }

        public async Task<OrderTypeChangeGetPagedListResponse> GetPagedListAsync(
            OrderTypeChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.OrderTypeChanges
                .AsNoTracking()
                .Where(x =>
                    (request.TypeId.IsEmpty() || x.TypeId == request.TypeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new OrderTypeChangeGetPagedListResponse
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
