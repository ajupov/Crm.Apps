using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Products.Models;

namespace Crm.Clients.Products.Clients
{
    public interface IProductsClient
    {
        Task<List<ProductType>> GetTypesAsync(CancellationToken ct = default);

        Task<Product> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Product>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Product>> GetPagedListAsync(Guid? accountId = default, Guid? parentProductId = default,
            List<ProductType> types = default, List<Guid> statusIds = default, string name = default,
            string vendorCode = default, decimal? minPrice = default, decimal? maxPrice = default,
            bool? isHidden = default, bool? isDeleted = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, bool? allCategoryIds = default,
            List<Guid> categoryIds = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(Product product, CancellationToken ct = default);

        Task UpdateAsync(Product product, CancellationToken ct = default);

        Task HideAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task ShowAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}