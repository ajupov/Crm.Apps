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

        public Task<ICollection<AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<AttributeType>>(
                $"{_settings.Host}/Api/UserAttributes/GetTypes", ct: ct);
        }

        public Task<UserAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<UserAttribute>(
                $"{_settings.Host}/Api/UserAttributes/Get", new {id}, ct);
        }

        public Task<ICollection<UserAttribute>> GetListAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<UserAttribute>>(
                $"{_settings.Host}/Api/UserAttributes/GetList", new {ids}, ct);
        }

        public Task<ICollection<UserAttribute>> GetPagedListAsync(Guid? accountId = default, bool? allTypes = default,
            ICollection<Type> types = default, string key = default, bool? isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ICollection<UserAttribute>>(
                $"{_settings.Host}/Api/UserAttributes/GetPagedList", new
                {
                    accountId, allTypes, types, key, isDeleted,
                    minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy
                }, ct);
        }

        public Task<Guid> CreateAsync(UserAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/UserAttributes/Create", attribute, ct);
        }

        public Task UpdateAsync(UserAttribute newAttribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserAttributes/Update", newAttribute, ct);
        }

        public Task DeleteAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserAttributes/Delete", ids, ct);
        }

        public Task RestoreAsync(ICollection<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/UserAttributes/Restore", ids, ct);
        }
    }
}