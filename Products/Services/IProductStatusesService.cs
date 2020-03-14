using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.Requests;
using Crm.Apps.Products.v1.Responses;

namespace Crm.Apps.Products.Services
{
    public interface IProductStatusesService
    {
        Task<ProductStatus> GetAsync(Guid id, CancellationToken ct);

        Task<List<ProductStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ProductStatusGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ProductStatusGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ProductStatus status, CancellationToken ct);

        Task UpdateAsync(Guid userId, ProductStatus oldStatus, ProductStatus newStatus, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}