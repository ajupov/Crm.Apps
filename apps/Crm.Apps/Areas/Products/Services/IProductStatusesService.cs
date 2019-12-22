using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.RequestParameters;

namespace Crm.Apps.Areas.Products.Services
{
    public interface IProductStatusesService
    {
        Task<ProductStatus> GetAsync(Guid id, CancellationToken ct);

        Task<List<ProductStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<ProductStatus>> GetPagedListAsync(
            ProductStatusGetPagedListRequestParameter request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ProductStatus status, CancellationToken ct);

        Task UpdateAsync(Guid userId, ProductStatus oldStatus, ProductStatus newStatus, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}