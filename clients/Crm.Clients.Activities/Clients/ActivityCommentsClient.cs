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
    public class ActivityCommentsClient : IActivityCommentsClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityCommentsClient(IOptions<ActivitiesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Activities/Comments");
            _httpClientFactory = httpClientFactory;
        }

        public Task<ActivityComment[]> GetPagedListAsync(
            ActivityCommentGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityComment[]>($"{_url}/GetPagedList", request, ct);
        }

        public Task CreateAsync(ActivityCommentCreateRequest request, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Create", request, ct);
        }
    }
}