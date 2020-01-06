using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.Settings;
using Crm.Common.All.Types.AttributeType;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Products.Clients
{
    public class ProductAttributesClient : IProductAttributesClient
    {
        private readonly ProductsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductAttributesClient(IOptions<ProductsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<AttributeType>>(
                $"{_settings.Host}/Api/Products/Attributes/GetTypes", ct: ct);
        }

        public Task<ProductAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ProductAttribute>(
                $"{_settings.Host}/Api/Products/Attributes/Get", new {id}, ct);
        }

        public Task<List<ProductAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ProductAttribute>>(
                $"{_settings.Host}/Api/Products/Attributes/GetList",
                ids, ct);
        }

        public Task<List<ProductAttribute>> GetPagedListAsync(Guid? accountId = default,
            List<AttributeType> types = default, string key = default, bool? isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Types = types,
                Key = key,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ProductAttribute>>(
                $"{_settings.Host}/Api/Products/Attributes/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(ProductAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Products/Attributes/Create", attribute,
                ct);
        }

        public Task UpdateAsync(ProductAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Attributes/Update", attribute, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Attributes/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Attributes/Restore", ids, ct);
        }
    }
}