using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;

namespace Crm.Apps.Clients.Products.Clients
{
    public interface IProductStatusesClient
    {
        Task<ProductStatus> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<ProductStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<ProductStatus>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ProductStatus group, CancellationToken ct = default);

        Task UpdateAsync(ProductStatus group, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}