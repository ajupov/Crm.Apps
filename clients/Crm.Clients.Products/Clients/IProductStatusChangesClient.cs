using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Products.Models;

namespace Crm.Clients.Products.Clients
{
    public interface IProductStatusChangesClient
    {
        Task<List<ProductCategoryChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? statusId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}