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
    public class UserGroupsClient : IUserGroupsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserGroupsClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Users/Groups");
            _httpClientFactory = httpClientFactory;
        }

        public Task<UserGroup> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<UserGroup>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<UserGroup>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<UserGroup>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<UserGroup>> GetPagedListAsync(
            UserGroupGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<UserGroup>>(UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task<Guid> CreateAsync(UserGroup group, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>(UriBuilder.Combine(_url, "Create"), group, ct);
        }

        public Task UpdateAsync(UserGroup group, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Update"), group, ct);
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