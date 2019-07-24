using System;
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
    public class IdentityChangesClient : IIdentityChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityChangesClient(
            IOptions<IdentitiesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Identities/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<IdentityChange[]> GetPagedListAsync(
            Guid identityId = default,
            Guid changerUserId = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = default,
            string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new IdentityChangeGetPagedListParameter(
                identityId, changerUserId, minCreateDate, maxCreateDate, offset, limit, sortBy, orderBy);

            return _httpClientFactory.PostAsync<IdentityChange[]>($"{_url}/GetPagedList", parameter, ct);
        }
    }
}