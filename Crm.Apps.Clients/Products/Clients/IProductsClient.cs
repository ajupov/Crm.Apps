using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.RequestParameters;

namespace Crm.Apps.Clients.Products.Clients
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