using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserGroups
{
    public class UserGroupsClient : IUserGroupsClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UserGroupsClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<UserGroup> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<UserGroup>($"{_settings.Host}/Api/UserGroups/Get", new {id}, ct);
        }

        public Task<ICollection<UserGroup>> GetListAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<UserGroup>>($"{_settings.Host}/Api/UserGroups/GetList",
                new {ids}, ct);
        }

        public Task<ICollection<UserGroup>> GetPagedListAsync(Guid? accountId = default, string name = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<UserGroup>>($"{_settings.Host}/Api/UserGroups/GetPagedList",
                new
                {
                    accountId, name, isDeleted, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy
                }, ct);
        }

        public Task<Guid> CreateAsync(UserGroup group, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/UserGroups/Create", group, ct);
        }

        public Task UpdateAsync(UserGroup group, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/UserGroups/Update", group, ct);
        }

        public Task DeleteAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserGroups/Delete", ids, ct);
        }

        public Task RestoreAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserGroups/Restore", ids, ct);
        }
    }
}