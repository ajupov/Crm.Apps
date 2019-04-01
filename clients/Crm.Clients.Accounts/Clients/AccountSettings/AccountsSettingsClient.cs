using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Accounts.Clients.AccountSettings
{
    public class AccountsSettingsClient : IAccountsSettingsClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AccountsClientSettings _settings;

        public AccountsSettingsClient(IOptions<AccountsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<ICollection<AccountSettingType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<AccountSettingType>>(
                $"{_settings.Host}/Api/Accounts/Settings", ct: ct);
        }
    }
}