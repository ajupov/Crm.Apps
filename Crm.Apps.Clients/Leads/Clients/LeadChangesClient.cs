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
    public class LeadChangesClient : ILeadChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadChangesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Leads/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<LeadChange>> GetPagedListAsync(
            LeadChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<LeadChange>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }
    }
}