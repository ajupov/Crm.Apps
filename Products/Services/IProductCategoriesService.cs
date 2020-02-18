using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.Requests;
using Crm.Apps.Products.v1.Responses;

namespace Crm.Apps.Products.Services
{
    public interface IProductCategoriesService
    {
        Task<ProductCategory> GetAsync(Guid id, CancellationToken ct);

        Task<List<ProductCategory>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<ProductCategoryGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            ProductCategoryGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ProductCategory category, CancellationToken ct);

        Task UpdateAsync(Guid userId, ProductCategory oldCategory, ProductCategory newCategory, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}