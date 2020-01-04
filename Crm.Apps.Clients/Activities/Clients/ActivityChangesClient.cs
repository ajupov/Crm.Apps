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
    public class ActivityChangesClient : IActivityChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityChangesClient(IOptions<ActivitiesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Activities/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<ActivityChange[]> GetPagedListAsync(
            ActivityChangeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityChange[]>($"{_url}/GetPagedList", request, ct);
        }
    }
}