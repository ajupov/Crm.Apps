using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.RequestParameters;
using Crm.Apps.Areas.Products.Storages;
using Crm.Apps.Utils;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Products.Services
{
    public class ProductCategoryChangesService : IProductCategoryChangesService
    {
        private readonly ProductsStorage _storage;

        public ProductCategoryChangesService(ProductsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<ProductCategoryChange>> GetPagedListAsync(
            ProductCategoryChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.ProductCategoryChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.CategoryId.IsEmpty() || x.CategoryId == request.CategoryId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}