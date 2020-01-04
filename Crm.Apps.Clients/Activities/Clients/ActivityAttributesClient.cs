using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;
using Crm.Apps.Clients.Activities.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Activities.Clients
{
    public class ActivityAttributesClient : IActivityAttributesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityAttributesClient(
            IOptions<ActivitiesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Api/Activities/Attributes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<Dictionary<string, AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Dictionary<string, AttributeType>>($"{_url}/GetTypes", ct: ct);
        }

        public Task<ActivityAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ActivityAttribute>($"{_url}/Get", new {id}, ct);
        }

        public Task<ActivityAttribute[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityAttribute[]>($"{_url}/GetList", ids, ct);
        }

        public Task<ActivityAttribute[]> GetPagedListAsync(
            ActivityAttributeGetPagedListRequest request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<ActivityAttribute[]>($"{_url}/GetPagedList", request, ct);
        }

        public Task<Guid> CreateAsync(ActivityAttributeCreateRequest request, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_url}/Create", request, ct);
        }

        public Task UpdateAsync(ActivityAttributeUpdateRequest request, CancellationToken ct = default)
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