using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Activities.Clients
{
    public class ActivityStatusChangesClient : IActivityStatusChangesClient
    {
        private readonly ActivitiesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityStatusChangesClient(IOptions<ActivitiesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<ActivityStatusChange>> GetPagedListAsync(Guid? changerUserId = default,
            Guid? sourceId = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerActivityId = changerUserId,
                StatusId = sourceId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ActivityStatusChange>>(
                $"{_settings.Host}/Api/Activities/Statuses/Changes/GetPagedList", parameter, ct);
        }
    }
}