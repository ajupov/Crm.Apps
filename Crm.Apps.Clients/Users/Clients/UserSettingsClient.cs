using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients
{
    public class UserSettingsClient : IUserSettingsClient
    {
        private readonly UsersClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserSettingsClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<UserSettingType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<UserSettingType>>($"{_settings.Host}/Api/Users/Settings/GetTypes",
                ct: ct);
        }
    }
}