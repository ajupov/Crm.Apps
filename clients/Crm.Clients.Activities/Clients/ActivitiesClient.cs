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
    public class ActivitiesClient : IActivitiesClient
    {
        private readonly ActivitiesClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivitiesClient(IOptions<ActivitiesClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<Activity> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Activity>($"{_settings.Host}/Api/Activities/Get", new {id}, ct);
        }

        public Task<List<Activity>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Activity>>($"{_settings.Host}/Api/Activities/GetList", ids, ct);
        }

        public Task<List<Activity>> GetPagedListAsync(Guid? accountId = default, string name = default,
            string description = default, string result = default, DateTime? minStartDateTime = default,
            DateTime? maxStartDateTime = default, DateTime? minEndDateTime = default,
            DateTime? maxEndDateTime = default, DateTime? minDeadLineDateTime = default,
            DateTime? maxDeadLineDateTime = default, bool isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, List<Guid> typeIds = default,
            List<Guid> statusIds = default, List<Guid> leadIds = default, List<Guid> companyIds = default,
            List<Guid> contactIds = default, List<Guid> dealIds = default, List<Guid> createUserIds = default,
            List<Guid> responsibleUserIds = default, int offset = default, int limit = 10, string sortBy = default,
            string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Name = name,
                Description = description,
                Result = result,
                MinStartDateTime = minStartDateTime,
                MaxStartDateTime = maxStartDateTime,
                MinEndDateTime = minEndDateTime,
                MaxEndDateTime = maxEndDateTime,
                MinDeadLineDateTime = minDeadLineDateTime,
                MaxDeadLineDateTime = maxDeadLineDateTime,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                TypeIds = typeIds,
                StatusIds = statusIds,
                LeadIds = leadIds,
                CompanyIds = companyIds,
                ContactIds = contactIds,
                DealIds = dealIds,
                CreateUserIds = createUserIds,
                ResponsibleUserIds = responsibleUserIds,
                AllAttributes = allAttributes,
                Attributes = attributes,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<Activity>>($"{_settings.Host}/Api/Activities/GetPagedList",
                parameter, ct);
        }

        public Task<Guid> CreateAsync(Activity activity, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Activities/Create", activity, ct);
        }

        public Task UpdateAsync(Activity activity, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Activities/Update", activity, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Activities/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Activities/Restore", ids, ct);
        }
    }
}