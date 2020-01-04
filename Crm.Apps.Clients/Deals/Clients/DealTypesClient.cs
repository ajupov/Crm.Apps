using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Deals.Clients
{
    public class DealTypesClient : IDealTypesClient
    {
        private readonly DealsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public DealTypesClient(IOptions<DealsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<DealType> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<DealType>($"{_settings.Host}/Api/Deals/Types/Get",
                new {id}, ct);
        }

        public Task<List<DealType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<DealType>>($"{_settings.Host}/Api/Deals/Types/GetList", ids,
                ct);
        }

        public Task<List<DealType>> GetPagedListAsync(Guid? accountId = default, string name = default,
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

            return _httpClientFactory.PostAsync<List<DealType>>($"{_settings.Host}/Api/Deals/Types/GetPagedList",
                parameter, ct);
        }

        public Task<Guid> CreateAsync(DealType type, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Deals/Types/Create", type, ct);
        }

        public Task UpdateAsync(DealType type, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Types/Update", type, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Types/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Types/Restore", ids, ct);
        }
    }
}