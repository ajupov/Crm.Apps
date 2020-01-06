using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.Settings;
using Crm.Common.All.Types.AttributeType;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Deals.Clients
{
    public class DealAttributesClient : IDealAttributesClient
    {
        private readonly DealsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public DealAttributesClient(IOptions<DealsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<AttributeType>>($"{_settings.Host}/Api/Deals/Attributes/GetTypes",
                ct: ct);
        }

        public Task<DealAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<DealAttribute>($"{_settings.Host}/Api/Deals/Attributes/Get", new {id},
                ct);
        }

        public Task<List<DealAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<DealAttribute>>($"{_settings.Host}/Api/Deals/Attributes/GetList",
                ids, ct);
        }

        public Task<List<DealAttribute>> GetPagedListAsync(Guid? accountId = default,
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

            return _httpClientFactory.PostAsync<List<DealAttribute>>(
                $"{_settings.Host}/Api/Deals/Attributes/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(DealAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Deals/Attributes/Create", attribute, ct);
        }

        public Task UpdateAsync(DealAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Attributes/Update", attribute, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Attributes/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Attributes/Restore", ids, ct);
        }
    }
}