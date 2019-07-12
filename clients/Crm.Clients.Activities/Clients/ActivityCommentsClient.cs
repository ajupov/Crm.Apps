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
    public class ActivityCommentsClient : IActivityCommentsClient
    {
        private readonly ActivitiesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityCommentsClient(IOptions<ActivitiesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<ActivityComment>> GetPagedListAsync(Guid? activityId = default,
            Guid? commentatorUserId = default, string value = default, DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                ActivityId = activityId,
                CommentatorUserId = commentatorUserId,
                Value = value,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ActivityComment>>(
                $"{_settings.Host}/Api/Activities/Comments/GetPagedList", parameter, ct);
        }

        public Task CreateAsync(ActivityComment comment, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Activities/Comments/Create", comment, ct);
        }
    }
}