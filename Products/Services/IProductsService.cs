using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.Requests;
using Crm.Apps.Products.v1.Responses;

namespace Crm.Apps.Products.Services
{
    public interface IProductsService
    {
        Task<Product> GetAsync(Guid id, CancellationToken ct);

        Task<List<Product>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ProductGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ProductGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Product product, CancellationToken ct);

        Task UpdateAsync(Guid userId, Product oldProduct, Product newProduct, CancellationToken ct);

        Task HideAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task ShowAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}