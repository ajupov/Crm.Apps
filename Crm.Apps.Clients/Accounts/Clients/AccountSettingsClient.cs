using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Accounts.Clients
{
    public class AccountSettingsClient : IAccountSettingsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountSettingsClient(IOptions<AccountsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Accounts/Settings");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<AccountSettingType, string>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<AccountSettingType, string>>($"{_url}/GetTypes", ct: ct);
        }
    }
}