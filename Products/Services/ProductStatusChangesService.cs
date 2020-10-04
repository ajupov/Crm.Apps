using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Products.Storages;
using Crm.Apps.Products.V1.Requests;
using Crm.Apps.Products.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Products.Services
{
    public class ProductStatusChangesService : IProductStatusChangesService
    {
        private readonly ProductsStorage _storage;

        public ProductStatusChangesService(ProductsStorage storage)
        {
            _storage = storage;
        }

        public async Task<ProductStatusChangeGetPagedListResponse> GetPagedListAsync(
            ProductStatusChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.ProductStatusChanges
                .AsNoTracking()
                .Where(x =>
                    (request.StatusId.IsEmpty() || x.StatusId == request.StatusId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new ProductStatusChangeGetPagedListResponse
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
