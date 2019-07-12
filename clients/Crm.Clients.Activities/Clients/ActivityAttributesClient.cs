using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Activities.Models;
using Crm.Clients.Activities.Settings;
using Crm.Common.Types;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Activities.Clients
{
    public class ActivityAttributesClient : IActivityAttributesClient
    {
        private readonly ActivitiesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityAttributesClient(IOptions<ActivitiesClientSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<AttributeType>> GetTypesAsync(CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<List<AttributeType>>(
                $"{_settings.Host}/Api/Activities/Attributes/GetTypes", ct: ct);
        }

        public Task<ActivityAttribute> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<ActivityAttribute>($"{_settings.Host}/Api/Activities/Attributes/Get",
                new {id}, ct);
        }

        public Task<List<ActivityAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ActivityAttribute>>(
                $"{_settings.Host}/Api/Activities/Attributes/GetList", ids, ct);
        }

        public Task<List<ActivityAttribute>> GetPagedListAsync(Guid? accountId = default,
            List<AttributeType> types = default, string key = default, bool? isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Types = types,
                Key = key,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<ActivityAttribute>>(
                $"{_settings.Host}/Api/Activities/Attributes/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(ActivityAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Activities/Attributes/Create", attribute,
                ct);
        }

        public Task UpdateAsync(ActivityAttribute attribute, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Activities/Attributes/Update", attribute, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Activities/Attributes/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Activities/Attributes/Restore", ids, ct);
        }
    }
}