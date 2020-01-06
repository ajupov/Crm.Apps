using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.RequestParameters;

namespace Crm.Apps.Clients.Products.Clients
{
    public interface IProductStatusesClient
    {
        Task<ProductStatus> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<ProductStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<ProductStatus>> GetPagedListAsync(
            ProductStatusGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ProductStatus group, CancellationToken ct = default);

        Task UpdateAsync(ProductStatus group, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}