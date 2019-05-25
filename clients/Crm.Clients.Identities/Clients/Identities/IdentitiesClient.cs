using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;
using Crm.Clients.Identities.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Identities.Clients.Identities
{
    public class IdentitiesClient : IIdentitiesClient
    {
        private readonly IdentitiesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentitiesClient(IOptions<IdentitiesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<IdentityType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<IdentityType>>($"{_settings.Host}/Api/Identities/GetTypes", ct: ct);
        }

        public Task<Identity> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Identity>($"{_settings.Host}/Api/Identities/Get", new {id}, ct);
        }

        public Task<List<Identity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Identity>>($"{_settings.Host}/Api/Identities/GetList", ids, ct);
        }

        public Task<List<Identity>> GetPagedListAsync(Guid? userId = default, List<IdentityType> types = default,
            string key = default, bool? isPrimary = default, bool? isVerified = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                UserId = userId,
                Types = types,
                Key = key,
                IsPrimary = isPrimary,
                IsVerified = isVerified,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<Identity>>($"{_settings.Host}/Api/Identities/GetPagedList",
                parameter, ct);
        }

        public Task<Guid> CreateAsync(Identity identity, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Identities/Create", identity, ct);
        }

        public Task UpdateAsync(Identity identity, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Identities/Update", identity, ct);
        }

        public Task VerifyAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Identities/Verify", ids, ct);
        }

        public Task UnverifyAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Identities/Unverify", ids, ct);
        }

        public Task SetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Identities/SetAsPrimary", ids, ct);
        }

        public Task ResetAsPrimaryAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Identities/ResetAsPrimary", ids, ct);
        }
    }
}