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
    public class UserAttributeChangesClient : IUserAttributeChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserAttributeChangesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Users/Attributes/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<UserAttributeChange>> GetPagedListAsync(
            UserAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<UserAttributeChange>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }
    }
}