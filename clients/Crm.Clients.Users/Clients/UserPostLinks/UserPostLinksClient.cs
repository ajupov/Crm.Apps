using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserPostLinks
{
    public class UserPostLinksClient : IUserPostLinksClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UserPostLinksClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task AddAsync(Guid userId, Guid postId, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserPostLinks/Set", new UserPostLink
            {
                UserId = userId,
                PostId = postId
            }, ct);
        }

        public Task DeleteAsync(Guid userId, Guid postId, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserPostLinks/Set", new UserPostLink
            {
                UserId = userId,
                PostId = postId
            }, ct);
        }
    }
}