using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Accounts.Clients.AccountsDefault
{
    public class AccountsDefaultClient : IAccountsDefaultClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AccountsClientSettings _settings;

        public AccountsDefaultClient(IOptions<AccountsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }


        public Task StatusAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync($"{_settings.Host}/Api/Accounts", ct: ct);
        }
    }
}