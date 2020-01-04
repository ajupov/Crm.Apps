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
    public class ActivityTypesClient : IActivityTypesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityTypesClient(IOptions<ActivitiesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Activities/Types");
            _httpClientFactory = httpClientFactory;
        }

        public Task<ActivityType> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ActivityType>($"{_url}/Get", new {id}, ct);
        }

        public Task<ActivityType[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityType[]>($"{_url}/GetList", ids, ct);
        }

        public Task<ActivityType[]> GetPagedListAsync(
            ActivityTypeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityType[]>($"{_url}/GetPagedList", request, ct);
        }

        public Task<Guid> CreateAsync(ActivityTypeCreateRequest request, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_url}/Create", request, ct);
        }

        public Task UpdateAsync(ActivityTypeUpdateRequest request, CancellationToken ct = default)
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