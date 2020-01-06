using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Users.Models;
using Crm.Apps.Clients.Users.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Users.Clients
{
    public class UsersClient : IUsersClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public UsersClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Users");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<string, UserGender>> GetGendersAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<string, UserGender>>(
                UriBuilder.Combine(_url, "GetGenders"), ct: ct);
        }

        public Task<User> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<User>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<User>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<User>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<User>> GetPagedListAsync(
            UserGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<User>>(UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task<Guid> CreateAsync(User user, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>(UriBuilder.Combine(_url, "Create"), user, ct);
        }

        public Task UpdateAsync(User user, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Update"), user, ct);
        }

        public Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Lock"), ids, ct);
        }

        public Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Unlock"), ids, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Delete"), ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Restore"), ids, ct);
        }
    }
}