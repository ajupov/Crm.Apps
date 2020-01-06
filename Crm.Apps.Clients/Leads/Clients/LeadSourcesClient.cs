using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Leads.Clients
{
    public class LeadSourcesClient : ILeadSourcesClient
    {
        private readonly LeadsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadSourcesClient(IOptions<LeadsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<LeadSource> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<LeadSource>($"{_settings.Host}/Api/Leads/Sources/Get",
                new {id}, ct);
        }

        public Task<List<LeadSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<LeadSource>>($"{_settings.Host}/Api/Leads/Sources/GetList", ids,
                ct);
        }

        public Task<List<LeadSource>> GetPagedListAsync(Guid? accountId = default, string name = default,
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

            return _httpClientFactory.PostAsync<List<LeadSource>>($"{_settings.Host}/Api/Leads/Sources/GetPagedList",
                parameter, ct);
        }

        public Task<Guid> CreateAsync(LeadSource source, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Leads/Sources/Create", source, ct);
        }

        public Task UpdateAsync(LeadSource source, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Leads/Sources/Update", source, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Leads/Sources/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Leads/Sources/Restore", ids, ct);
        }
    }
}