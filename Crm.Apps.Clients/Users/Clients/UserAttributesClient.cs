using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Users.Models;
using Crm.Apps.Clients.Users.RequestParameters;
using Crm.Common.All.Types.AttributeType;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Users.Clients
{
    public class UserAttributesClient : IUserAttributesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserAttributesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Users/Attributes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<string, AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<string, AttributeType>>(
                UriBuilder.Combine(_url, "GetTypes"), ct: ct);
        }

        public Task<UserAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<UserAttribute>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<UserAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<UserAttribute>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<UserAttribute>> GetPagedListAsync(
            UserAttributeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<UserAttribute>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task<Guid> CreateAsync(UserAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>(UriBuilder.Combine(_url, "Create"), attribute, ct);
        }

        public Task UpdateAsync(UserAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Update"), attribute, ct);
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