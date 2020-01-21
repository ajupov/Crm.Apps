using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.v1.Clients.Leads.Models;
using Crm.Apps.v1.Clients.Leads.RequestParameters;
using Crm.Common.All.Types.AttributeType;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.v1.Clients.Leads.Clients
{
    public class LeadAttributesClient : ILeadAttributesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadAttributesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.ApiHost, "Leads/Attributes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<string, AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<string, AttributeType>>(
                UriBuilder.Combine(_url, "GetTypes"), ct: ct);
        }

        public Task<LeadAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<LeadAttribute>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<LeadAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<LeadAttribute>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<LeadAttribute>> GetPagedListAsync(
            LeadAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<LeadAttribute>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task<Guid> CreateAsync(LeadAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Guid>(UriBuilder.Combine(_url, "Create"), attribute, ct);
        }

        public Task UpdateAsync(LeadAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync(UriBuilder.Combine(_url, "Update"), attribute, ct);
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