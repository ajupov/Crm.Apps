using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Accounts.Models;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Accounts.Clients
{
    public class AccountSettingsClient : IAccountSettingsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountSettingsClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Accounts/Settings");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<AccountSettingType, string>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<AccountSettingType, string>>(
                UriBuilder.Combine(_url, "GetTypes"), ct: ct);
        }
    }
}