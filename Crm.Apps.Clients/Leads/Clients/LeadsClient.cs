using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Leads.Clients
{
    public class LeadsClient : ILeadsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadsClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Leads");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Lead> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Lead>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<Lead>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Lead>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<Lead>> GetPagedListAsync(
            LeadGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Lead>>(UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task<Guid> CreateAsync(Lead lead, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Guid>(UriBuilder.Combine(_url, "Create"), lead, ct);
        }

        public Task UpdateAsync(Lead lead, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync(UriBuilder.Combine(_url, "Update"), lead, ct);
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