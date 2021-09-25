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
    public class OrderChangesService : IOrderChangesService
    {
        private readonly OrdersStorage _storage;

        public OrderChangesService(OrdersStorage storage)
        {
            _storage = storage;
        }

        public async Task<OrderChangeGetPagedListResponse> GetPagedListAsync(
            OrderChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.OrderChanges
                .AsNoTracking()
                .Where(x =>
                    (request.OrderId.IsEmpty() || x.OrderId == request.OrderId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new OrderChangeGetPagedListResponse
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
