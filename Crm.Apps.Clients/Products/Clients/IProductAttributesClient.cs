using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.RequestParameters;
using Crm.Common.All.Types.AttributeType;

namespace Crm.Apps.Clients.Products.Clients
{
    public interface IProductAttributesClient
    {
        Task<Dictionary<string, AttributeType>> GetTypesAsync(CancellationToken ct = default);

        Task<ProductAttribute> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<ProductAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<ProductAttribute>> GetPagedListAsync(
            ProductAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(ProductAttribute attribute, CancellationToken ct = default);

        Task UpdateAsync(ProductAttribute attribute, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}