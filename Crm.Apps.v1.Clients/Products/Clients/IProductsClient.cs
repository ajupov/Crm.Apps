using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Apps.v1.Clients.Products.RequestParameters;

namespace Crm.Apps.v1.Clients.Products.Clients
{
    public interface IProductsClient
    {
        Task<Dictionary<string, ProductType>> GetTypesAsync(CancellationToken ct = default);

        Task<Product> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Product>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Product>> GetPagedListAsync(
            ProductGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(Product product, CancellationToken ct = default);

        Task UpdateAsync(Product product, CancellationToken ct = default);

        Task HideAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task ShowAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}