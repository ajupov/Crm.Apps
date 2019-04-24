using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserPosts
{
    public class UserPostsClient : IUserPostsClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UserPostsClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<UserPost> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<UserPost>($"{_settings.Host}/Api/UserPosts/Get", new {id}, ct);
        }

        public Task<ICollection<UserPost>> GetListAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<UserPost>>($"{_settings.Host}/Api/UserPosts/GetList",
                new {ids}, ct);
        }

        public Task<ICollection<UserPost>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<UserPost>>($"{_settings.Host}/Api/UserPosts/GetPagedList",
                new
                {
                    accountId, name, isDeleted, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy
                }, ct);
        }

        public Task<Guid> CreateAsync(UserPost post, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/UserPosts/Create", post, ct);
        }

        public Task UpdateAsync(UserPost post, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/UserPosts/Update", post, ct);
        }

        public Task DeleteAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserPosts/Delete", ids, ct);
        }

        public Task RestoreAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserPosts/Restore", ids, ct);
        }
    }
}