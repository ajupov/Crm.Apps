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
    public class ActivityStatusChangesClient : IActivityStatusChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityStatusChangesClient(
            IOptions<ActivitiesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Activities/Statuses/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<ActivityStatusChange[]> GetPagedListAsync(
            ActivityStatusChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityStatusChange[]>($"{_url}/GetPagedList", request, ct);
        }
    }
}