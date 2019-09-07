using System;
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
    public class ActivityStatusesClient : IActivityStatusesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityStatusesClient(IOptions<ActivitiesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Activities/Statuses");
            _httpClientFactory = httpClientFactory;
        }

        public Task<ActivityStatus> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ActivityStatus>($"{_url}/Get", new {id}, ct);
        }

        public Task<ActivityStatus[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityStatus[]>($"{_url}/GetList", ids, ct);
        }

        public Task<ActivityStatus[]> GetPagedListAsync(
            ActivityStatusGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityStatus[]>($"{_url}/GetPagedList", request, ct);
        }

        public Task<Guid> CreateAsync(ActivityStatusCreateRequest request, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_url}/Create", request, ct);
        }

        public Task UpdateAsync(ActivityStatusUpdateRequest request, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Update", request, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_url}/Restore", ids, ct);
        }
    }
}