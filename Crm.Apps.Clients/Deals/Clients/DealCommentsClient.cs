using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Deals.Models;
using Crm.Clients.Deals.Settings;
using Crm.Utils.Http;
using Microsoft.Extensions.Options;

namespace Crm.Clients.Deals.Clients
{
    public class DealCommentsClient : IDealCommentsClient
    {
        private readonly DealsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public DealCommentsClient(IOptions<DealsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _settings = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<DealComment>> GetPagedListAsync(Guid? dealId = default, Guid? commentatorUserId = default,
            string value = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                DealId = dealId,
                CommentatorUserId = commentatorUserId,
                Value = value,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<DealComment>>($"{_settings.Host}/Api/Deals/Comments/GetPagedList",
                parameter, ct);
        }

        public Task CreateAsync(DealComment comment, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Deals/Comments/Create", comment, ct);
        }
    }
}