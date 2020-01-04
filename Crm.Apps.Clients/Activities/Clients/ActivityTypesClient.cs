using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Activities.Models;
using Crm.Apps.Clients.Activities.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Activities.Clients
{
    public class ActivityTypesClient : IActivityTypesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityTypesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Activities/Types");
            _httpClientFactory = httpClientFactory;
        }

        public Task<ActivityType> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ActivityType>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<ActivityType>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ActivityType>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<ActivityType>> GetPagedListAsync(
            ActivityTypeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ActivityType>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }

        public Task<Guid> CreateAsync(ActivityType type, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>(UriBuilder.Combine(_url, "Create"), type, ct);
        }

        public Task UpdateAsync(ActivityType type, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Update"), type, ct);
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