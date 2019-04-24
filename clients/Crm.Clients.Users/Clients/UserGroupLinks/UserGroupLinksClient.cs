using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserGroupLinks
{
    public class UserGroupLinksClient : IUserGroupLinksClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UserGroupLinksClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task AddToGroupAsync(Guid userId, Guid groupId, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserGroupLinks/Set", new UserGroupLink
            {
                UserId = userId,
                GroupId = groupId
            }, ct);
        }

        public Task DeleteFromGroupAsync(Guid userId, Guid groupId, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserGroupLinks/Reset", new UserGroupLink
            {
                UserId = userId,
                GroupId = groupId
            }, ct);
        }
    }
}