using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.RequestParameters;
using Crm.Clients.Activities.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;
using UriBuilder = Crm.Utils.Http.UriBuilder;

namespace Crm.Clients.Activities.Clients
{
    public class ActivityAttributeChangesClient : IActivityAttributeChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityAttributeChangesClient(
            IOptions<ActivitiesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Activities/Attributes/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<ActivityAttributeChange>> GetPagedListAsync(
            ActivityAttributeChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ActivityAttributeChange>>($"{_url}/GetPagedList", request, ct);
        }
    }
}