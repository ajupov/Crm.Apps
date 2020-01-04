using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;
using Crm.Apps.Products.Storages;
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

        public Task<List<ProductStatusChange>> GetPagedListAsync(
            ProductStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.ProductStatusChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.StatusId.IsEmpty() || x.StatusId == request.StatusId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}