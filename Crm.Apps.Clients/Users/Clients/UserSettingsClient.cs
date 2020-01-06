using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Users.Models;
using Crm.Apps.Clients.Users.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Users.Clients
{
    public class UserSettingsClient : IUserSettingsClient
    {
        private readonly UsersClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserSettingsClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<UserSettingType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<UserSettingType>>($"{_settings.Host}/Api/Users/Settings/GetTypes",
                ct: ct);
        }
    }
}