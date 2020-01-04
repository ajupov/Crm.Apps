using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;

namespace Crm.Apps.Products.Services
{
    public interface IProductCategoriesService
    {
        Task<ProductCategory> GetAsync(Guid id, CancellationToken ct);

        Task<List<ProductCategory>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<ProductCategory>> GetPagedListAsync(
            ProductCategoryGetPagedListRequestParameter request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ProductCategory category, CancellationToken ct);

        Task UpdateAsync(Guid userId, ProductCategory oldCategory, ProductCategory newCategory, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}