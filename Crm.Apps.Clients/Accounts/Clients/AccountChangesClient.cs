using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Accounts.Models;
using Crm.Apps.Clients.Accounts.RequestParameters;
using Crm.Apps.Clients.Accounts.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Accounts.Clients
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