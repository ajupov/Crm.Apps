using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Parameters;

namespace Crm.Apps.Products.Services
{
    public interface IProductAttributesService
    {
        Task<ProductAttribute> GetAsync(Guid id, CancellationToken ct);

        Task<List<ProductAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<ProductAttribute>> GetPagedListAsync(ProductAttributeGetPagedListParameter parameter,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, ProductAttribute attribute, CancellationToken ct);

        Task UpdateAsync(Guid userId, ProductAttribute oldAttribute, ProductAttribute newAttribute,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}