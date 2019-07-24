using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Identities.Models;
using Crm.Clients.Identities.Parameters;
using Crm.Clients.Identities.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;
using UriBuilder = Crm.Utils.Http.UriBuilder;

namespace Crm.Clients.Identities.Clients
{
    public class IdentitiesClient : IIdentitiesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentitiesClient(
            IOptions<IdentitiesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Identities");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<string, IdentityType>> GetTypesAsync(
            CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<string, IdentityType>>($"{_url}/GetTypes", ct: ct);
        }

        public Task<Identity> GetAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Identity>($"{_url}/Get", new {id}, ct);
        }

        public Task<Identity> GetByKeyAndTypesAsync(
            string key,
            IdentityType[] types,
            CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Identity>($"{_url}/Get", new {key}, ct);
        }

        public Task<Identity[]> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Identity[]>($"{_url}/GetList", ids, ct);
        }

        public Task<Identity[]> GetPagedListAsync(
            Guid? userId = default,
            IdentityType[] types = default,
            string key = default,
            bool? isPrimary = default,
            bool? isVerified = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc",
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

            return _httpClientFactory.PostAsync<Identity[]>($"{_url}/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(
            Identity identity,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_url}/Create", identity, ct);
        }

        public Task UpdateAsync(
            Identity identity,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Update", identity, ct);
        }

        public Task SetPasswordAsync(
            Guid id,
            string password,
            CancellationToken ct = default)
        {
            var parameter = new SetPasswordParameter(id, password);

            return _httpClientFactory.PostAsync($"{_url}/SetPassword", parameter, ct);
        }

        public Task<bool> IsPasswordCorrectAsync(
            string key,
            string password,
            CancellationToken ct = default)
        {
            var parameter = new IsPasswordCorrectParameter(key, password);
            
            return _httpClientFactory.GetAsync<bool>($"{_url}/IsPasswordCorrect", parameter, ct);
        }

        public Task VerifyAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Verify", ids, ct);
        }

        public Task UnverifyAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Unverify", ids, ct);
        }

        public Task SetAsPrimaryAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/SetAsPrimary", ids, ct);
        }

        public Task ResetAsPrimaryAsync(
            IEnumerable<Guid> ids,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/ResetAsPrimary", ids, ct);
        }
    }
}