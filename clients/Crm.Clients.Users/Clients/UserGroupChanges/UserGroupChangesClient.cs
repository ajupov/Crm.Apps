using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserGroupChanges
{
    public class UserGroupChangesClient : IUserGroupChangesClient
    {
        private readonly UsersClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserGroupChangesClient(IHttpClientFactory httpClientFactory, IOptions<UsersClientSettings> options)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<List<UserGroupChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? groupId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = 10, int limit = default,
            string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<UserGroupChange>>(
                $"{_settings.Host}/Api/Users/Groups/Changes/GetPagedList",
                new {changerUserId, groupId, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy}, ct);
        }
    }
}