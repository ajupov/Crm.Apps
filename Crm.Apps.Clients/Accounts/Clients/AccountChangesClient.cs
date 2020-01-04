using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.RequestParameters;
using Crm.Clients.Accounts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;
using UriBuilder = Crm.Utils.Http.UriBuilder;

namespace Crm.Clients.Accounts.Clients
{
    public class AccountChangesClient : IAccountChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountChangesClient(IOptions<AccountsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Accounts/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<AccountChange[]> GetPagedListAsync(
            AccountChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<AccountChange[]>($"{_url}/GetPagedList", request, ct);
        }
    }
}