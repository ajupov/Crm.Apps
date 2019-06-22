using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Products.Settings;
using Crm.Clients.Products.Models;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Products.Clients
{
    public class ProductsClient : IProductsClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ProductsClientSettings _settings;

        public ProductsClient(IOptions<ProductsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<List<ProductType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<ProductType>>($"{_settings.Host}/Api/Products/GetTypes", ct: ct);
        }

        public Task<Product> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Product>($"{_settings.Host}/Api/Products/Get", new {id}, ct);
        }

        public Task<List<Product>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Product>>($"{_settings.Host}/Api/Products/GetList", ids, ct);
        }

        public Task<List<Product>> GetPagedListAsync(Guid? accountId = default, Guid? parentProductId = default,
            List<ProductType> types = default, List<Guid> statusIds = default, string name = default,
            string vendorCode = default, decimal? minPrice = default, decimal? maxPrice = default,
            bool? isHidden = default, bool? isDeleted = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, bool? allCategoryIds = default,
            List<Guid> categoryIds = default, int? offset = default, int? limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                ParentProductId = parentProductId,
                Types = types,
                StatusIds = statusIds,
                Name = name,
                VendorCode = vendorCode,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                IsHidden = isHidden,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                AllAttributes = allAttributes,
                Attributes = attributes,
                AllCategoryIds = allCategoryIds,
                CategoryIds = categoryIds,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<Product>>($"{_settings.Host}/Api/Products/GetPagedList", parameter,
                ct);
        }

        public Task<Guid> CreateAsync(Product user, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Products/Create", user, ct);
        }

        public Task UpdateAsync(Product user, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Update", user, ct);
        }

        public Task HideAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Hide", ids, ct);
        }

        public Task ShowAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Show", ids, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Restore", ids, ct);
        }
    }
}