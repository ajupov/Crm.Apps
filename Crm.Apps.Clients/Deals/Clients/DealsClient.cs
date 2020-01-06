using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Deals.Clients
{
    public class DealsClient : IDealsClient
    {
        private readonly DealsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public DealsClient(IOptions<DealsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<Deal> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<Deal>($"{_settings.Host}/Api/Deals/Get", new {id}, ct);
        }

        public Task<List<Deal>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<Deal>>($"{_settings.Host}/Api/Deals/GetList", ids, ct);
        }

        public Task<List<Deal>> GetPagedListAsync(Guid? accountId = default, string name = default,
            DateTime? minStartDateTime = default, DateTime? maxStartDateTime = default,
            DateTime? minEndDateTime = default, DateTime? maxEndDateTime = default, decimal? minSum = default,
            decimal? maxSum = default, decimal? minSumWithoutDiscount = default, decimal? maxSumWithoutDiscount = default,
            byte? minFinishProbability = default, byte? maxFinishProbability = default, bool isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, List<Guid> typeIds = default,
            List<Guid> statusIds = default, List<Guid> companyIds = default, List<Guid> contactIds = default,
            List<Guid> createUserIds = default, List<Guid> responsibleUserIds = default, bool? allAttributes = default,
            IDictionary<Guid, string> attributes = default, List<Guid> positionsProductIds = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                AccountId = accountId,
                Name = name,
                MinStartDateTime = minStartDateTime,
                MaxStartDateTime = maxStartDateTime,
                MinEndDateTime = minEndDateTime,
                MaxEndDateTime = maxEndDateTime,
                MinSum = minSum,
                MaxSum = maxSum,
                MinSumWithoutDiscount = minSumWithoutDiscount,
                MaxSumWithoutDiscount = maxSumWithoutDiscount,
                MinFinishProbability = minFinishProbability,
                MaxFinishProbability = maxFinishProbability,
                IsDeleted = isDeleted,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                TypeIds = typeIds,
                StatusIds = statusIds,
                CompanyIds = companyIds,
                ContactIds = contactIds,
                CreateUserIds = createUserIds,
                ResponsibleUserIds = responsibleUserIds,
                AllAttributes = allAttributes,
                Attributes = attributes,
                PositionsProductIds = positionsProductIds,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<Deal>>($"{_settings.Host}/Api/Deals/GetPagedList", parameter, ct);
        }

        public Task<Guid> CreateAsync(Deal deal, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>($"{_settings.Host}/Api/Deals/Create", deal, ct);
        }

        public Task UpdateAsync(Deal deal, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Update", deal, ct);
        }

        public Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Delete", ids, ct);
        }

        public Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Restore", ids, ct);
        }
    }
}