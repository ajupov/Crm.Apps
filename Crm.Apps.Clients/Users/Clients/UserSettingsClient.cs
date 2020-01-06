using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Users.Models;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Users.Clients
{
    public class UserSettingsClient : IUserSettingsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserSettingsClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Users/Settings");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<string, UserSettingType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<string, UserSettingType>>(
                UriBuilder.Combine(_url, "GetTypes"), ct: ct);
        }
    }
}