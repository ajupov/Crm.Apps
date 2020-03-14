using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.Requests;
using Crm.Apps.Products.v1.Responses;

namespace Crm.Apps.Products.Services
{
    public interface IProductAttributesService
    {
        Task<ProductAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<ProductAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ProductAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ProductAttributeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ProductAttribute attribute, CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            ProductAttribute oldAttribute,
            ProductAttribute newAttribute,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}