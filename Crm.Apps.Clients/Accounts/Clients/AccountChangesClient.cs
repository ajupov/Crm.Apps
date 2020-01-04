using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Accounts.Models;
using Crm.Apps.Clients.Accounts.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Accounts.Clients
{
    public class AccountChangesClient : IAccountChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountChangesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Accounts/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<AccountChange>> GetPagedListAsync(
            AccountChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<AccountChange>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }
    }
}