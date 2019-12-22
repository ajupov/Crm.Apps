using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Helpers;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Parameters;
using Crm.Apps.Areas.Products.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Products.Services
{
    public class ProductChangesService : IProductChangesService
    {
        private readonly ProductsStorage _storage;

        public ProductChangesService(ProductsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<ProductChange>> GetPagedListAsync(ProductChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.ProductChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.ProductId.IsEmpty() || x.ProductId == parameter.ProductId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}