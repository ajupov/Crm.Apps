using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Users.Models;
using Crm.Clients.Users.Settings;
using Crm.Common.Types;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Users.Clients.UserAttributes
{
    public class UserAttributesClient : IUserAttributesClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UsersClientSettings _settings;

        public UserAttributesClient(IOptions<UsersClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _settings = options.Value;
        }

        public Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<AttributeType>>(
                $"{_settings.Host}/Api/Users/Attributes/GetTypes", ct: ct);
        }

        public Task<UserAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<UserAttribute>(
                $"{_settings.Host}/Api/Users/Attributes/Get", new {id}, ct);
        }

        public Task<List<UserAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<UserAttribute>>(
                $"{_settings.Host}/Api/Users/Attributes/GetList", new {ids}, ct);
        }

        public Task<List<UserAttribute>> GetPagedListAsync(Guid? accountId = default, bool? allTypes = default,
            List<Type> types = default, string key = default, bool? isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<UserAttribute>>(
                $"{_settings.Host}/Api/Users/Attributes/GetPagedList", new
                {
                    accountId, allTypes, types, key, isDeleted,
                    minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy
                }, ct);
        }

        public Task<Guid> CreateAsync(UserAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Users/Attributes/Create", attribute, ct);
        }

        public Task UpdateAsync(UserAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Users/Attributes/Update", attribute, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Users/Attributes/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Users/Attributes/Restore", ids, ct);
        }
    }
}