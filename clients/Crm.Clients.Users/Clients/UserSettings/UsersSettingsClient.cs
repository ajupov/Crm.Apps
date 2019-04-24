using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserSettings
{
    public class UsersSettingsClient : IUsersSettingsClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UsersSettingsClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<ICollection<UserSettingType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<UserSettingType>>(
                $"{_settings.Host}/Api/Users/Settings/GetTypes", ct: ct);
        }
    }
}