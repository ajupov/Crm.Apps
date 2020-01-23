﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Apps.v1.Clients.Products.RequestParameters;

namespace Crm.Apps.v1.Clients.Products.Clients
{
    public interface IProductCategoriesClient
    {
        Task<ProductCategory> GetAsync(string accessToken, Guid id, CancellationToken ct = default);

        Task<List<ProductCategory>> GetListAsync(
            string accessToken,
            IEnumerable<Guid> ids,
            CancellationToken ct = default);

        Task<List<ProductCategory>> GetPagedListAsync(
            string accessToken,
            ProductCategoryGetPagedListRequestParameter request,
            CancellationToken ct = default);

        Task<Guid> CreateAsync(string accessToken, ProductCategory group, CancellationToken ct = default);

        Task UpdateAsync(string accessToken, ProductCategory group, CancellationToken ct = default);

        Task DeleteAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(string accessToken, IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}