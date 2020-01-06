using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Products.Clients
{
    public class ProductStatusChangesClient : IProductStatusChangesClient
    {
        private readonly ProductsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductStatusChangesClient(IOptions<ProductsClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<ProductStatusChange>> GetPagedListAsync(Guid? changerUserId = default,
            Guid? statusId = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerProductId = changerUserId,
                StatusId = statusId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ProductStatusChange>>(
                $"{_settings.Host}/Api/Products/Statuses/Changes/GetPagedList", parameter, ct);
        }
    }
}