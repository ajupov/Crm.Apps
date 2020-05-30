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
    public class ProductChangesService : IProductChangesService
    {
        private readonly ProductsStorage _storage;

        public ProductChangesService(ProductsStorage storage)
        {
            _storage = storage;
        }

        public async Task<ProductChangeGetPagedListResponse> GetPagedListAsync(
            ProductChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.ProductChanges
                .Where(x =>
                    (request.ProductId.IsEmpty() || x.ProductId == request.ProductId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new ProductChangeGetPagedListResponse
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
