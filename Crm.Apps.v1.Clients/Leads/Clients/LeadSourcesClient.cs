using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.v1.Clients.Leads.Clients
{
    public class LeadSourcesClient : ILeadSourcesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadSourcesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.ApiHost, "Leads/Sources");
            _httpClientFactory = httpClientFactory;
        }

        public Task<LeadSource> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<LeadSource>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<LeadSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<LeadSource>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<LeadSource>> GetPagedListAsync(
            LeadSourceGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<LeadSource>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task<Guid> CreateAsync(LeadSource source, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Guid>(UriBuilder.Combine(_url, "Create"), source, ct);
        }

        public Task UpdateAsync(LeadSource source, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync(UriBuilder.Combine(_url, "Update"), source, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync(UriBuilder.Combine(_url, "Delete"), ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync(UriBuilder.Combine(_url, "Restore"), ids, ct);
        }
    }
}