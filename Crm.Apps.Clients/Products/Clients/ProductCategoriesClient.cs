﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Products.Clients
{
    public class ProductCategoriesClient : IProductCategoriesClient
    {
        private readonly ProductsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductCategoriesClient(IOptions<ProductsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<ProductCategory> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ProductCategory>($"{_settings.Host}/Api/Products/Categories/Get",
                new {id}, ct);
        }

        public Task<List<ProductCategory>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ProductCategory>>(
                $"{_settings.Host}/Api/Products/Categories/GetList", ids, ct);
        }

        public Task<List<ProductCategory>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Name = name,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ProductCategory>>(
                $"{_settings.Host}/Api/Products/Categories/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(ProductCategory category, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Products/Categories/Create", category, ct);
        }

        public Task UpdateAsync(ProductCategory category, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Categories/Update", category, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Categories/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Products/Categories/Restore", ids, ct);
        }
    }
}