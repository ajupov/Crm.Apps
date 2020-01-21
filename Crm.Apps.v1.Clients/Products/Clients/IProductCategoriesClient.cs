using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Apps.v1.Clients.Products.RequestParameters;

namespace Crm.Apps.v1.Clients.Products.Clients
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