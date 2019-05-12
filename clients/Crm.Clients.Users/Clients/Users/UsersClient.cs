using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Common.UserContext;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.Users
{
    public class UsersClient : IUsersClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UsersClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<List<UserGender>> GetGendersAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<UserGender>>($"{_settings.Host}/Api/Users/GetGenders",
                ct: ct);
        }

        public Task<User> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<User>($"{_settings.Host}/Api/Users/Get", new {id}, ct);
        }

        public Task<List<User>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<User>>($"{_settings.Host}/Api/Users/GetList",
                new {ids}, ct);
        }

        public Task<List<User>> GetPagedListAsync(Guid? accountId = default, string surname = default,
            string name = default, string patronymic = default, DateTime? minBirthDate = default,
            DateTime? maxBirthDate = default, UserGender? gender = default, bool? isLocked = default,
            bool? isDeleted = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            bool? allAttributes = default, IDictionary<Guid, string> attributes = default,
            bool? allPermissions = default, List<Permission> permissions = default, bool? allGroupIds = default,
            List<Guid> groupIds = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<User>>($"{_settings.Host}/Api/Users/GetPagedList", new
            {
                accountId, surname, name, patronymic, minBirthDate, maxBirthDate, gender, isLocked, isDeleted,
                minCreateDate, maxCreateDate, allAttributes, attributes, allPermissions, permissions, allGroupIds,
                groupIds, offset, limit, sortBy, orderBy
            }, ct);
        }

        public Task<Guid> CreateAsync(User user, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Users/Create", user, ct);
        }

        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Users/Update", user, ct);
        }

        public Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Users/Lock", ids, ct);
        }

        public Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Users/Unlock", ids, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Users/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Users/Restore", ids, ct);
        }
    }
}