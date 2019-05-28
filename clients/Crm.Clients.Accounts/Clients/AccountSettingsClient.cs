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
        private readonly AccountsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountSettingsClient(IOptions<AccountsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<AccountSettingType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<AccountSettingType>>(
                $"{_settings.Host}/Api/Accounts/Settings/GetTypes", ct: ct);
        }
    }
}