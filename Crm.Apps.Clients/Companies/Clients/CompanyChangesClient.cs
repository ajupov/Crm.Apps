using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Companies.Models;
using Crm.Apps.Clients.Companies.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Companies.Clients
{
    public class CompanyChangesClient : ICompanyChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyChangesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Companies/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<CompanyChange>> GetPagedListAsync(
            CompanyChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<CompanyChange>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }
    }
}