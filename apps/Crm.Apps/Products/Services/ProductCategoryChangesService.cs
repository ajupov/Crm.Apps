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
    public class ProductCategoryChangesService : IProductCategoryChangesService
    {
        private readonly ProductsStorage _storage;

        public ProductCategoryChangesService(ProductsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<ProductCategoryChange>> GetPagedListAsync(ProductCategoryChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.ProductCategoryChanges.Where(x =>
                    (!parameter.ChangerUserId.HasValue || x.ChangerUserId == parameter.ChangerUserId) &&
                    (!parameter.CategoryId.HasValue || x.CategoryId == parameter.CategoryId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}