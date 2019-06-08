using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Products.Models;
using Crm.Clients.Products.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Products.Clients
{
    public class ProductCategoryChangesClient : IProductCategoryChangesClient
    {
        private readonly ProductsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductCategoryChangesClient(IHttpClientFactory httpClientFactory,
            IOptions<ProductsClientSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<List<ProductCategoryChange>> GetPagedListAsync(Guid? changerUserId = default,
            Guid? categoryId = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerProductId = changerUserId,
                CategoryId = categoryId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ProductCategoryChange>>(
                $"{_settings.Host}/Api/Products/Categories/Changes/GetPagedList", parameter, ct);
        }
    }
}