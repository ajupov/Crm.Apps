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
    public class LeadCommentsClient : ILeadCommentsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadCommentsClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Leads/Comments");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<LeadComment>> GetPagedListAsync(
            LeadCommentGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<LeadComment>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task CreateAsync(LeadComment comment, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Create"), comment, ct);
        }
    }
}