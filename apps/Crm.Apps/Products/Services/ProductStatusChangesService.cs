using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Helpers;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Parameters;
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

        public Task<List<ProductStatusChange>> GetPagedListAsync(ProductStatusChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.ProductStatusChanges.Where(x =>
                    (!parameter.ChangerUserId.HasValue || x.ChangerUserId == parameter.ChangerUserId) &&
                    (!parameter.StatusId.HasValue || x.StatusId == parameter.StatusId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}