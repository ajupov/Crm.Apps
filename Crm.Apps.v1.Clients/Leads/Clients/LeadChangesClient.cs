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
    public class LeadChangesClient : ILeadChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadChangesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.ApiHost, "Leads/Changes/v1");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<LeadChange>> GetPagedListAsync(
            string accessToken,
            LeadChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostJsonAsync<List<LeadChange>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, accessToken, ct);
        }
    }
}