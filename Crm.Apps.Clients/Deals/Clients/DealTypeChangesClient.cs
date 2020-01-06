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
    public class DealTypeChangesClient : IDealTypeChangesClient
    {
        private readonly DealsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public DealTypeChangesClient(IOptions<DealsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<DealTypeChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? typeId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default, int limit = 10,
            string sortBy = default, string orderBy = default, CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerDealId = changerUserId,
                TypeId = typeId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<DealTypeChange>>(
                $"{_settings.Host}/Api/Deals/Types/Changes/GetPagedList", parameter, ct);
        }
    }
}