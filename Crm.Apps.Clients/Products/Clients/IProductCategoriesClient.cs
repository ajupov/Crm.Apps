using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.RequestParameters;

namespace Crm.Apps.Clients.Products.Clients
{
    public interface IProductCategoriesClient
    {
        Task<ProductCategory> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<ProductCategory>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<ProductCategory>> GetPagedListAsync(
            ProductCategoryGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ProductCategory group, CancellationToken ct = default);

        Task UpdateAsync(ProductCategory group, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}