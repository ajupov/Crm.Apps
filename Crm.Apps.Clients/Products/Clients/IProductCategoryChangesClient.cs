using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;

namespace Crm.Apps.Clients.Products.Clients
{
    public interface IProductCategoryChangesClient
    {
        Task<List<ProductCategoryChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? categoryId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}