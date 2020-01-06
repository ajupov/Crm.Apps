using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Leads.Models;
using Crm.Apps.Clients.Leads.Settings;
using Microsoft.Extensions.Options;

namespace Crm.Apps.Clients.Leads.Clients
{
    public class LeadCommentsClient : ILeadCommentsClient
    {
        private readonly LeadsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadCommentsClient(IOptions<LeadsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<LeadComment>> GetPagedListAsync(Guid? leadId = default, Guid? commentatorUserId = default,
            string value = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                LeadId = leadId,
                CommentatorUserId = commentatorUserId,
                Value = value,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<LeadComment>>($"{_settings.Host}/Api/Leads/Comments/GetPagedList",
                parameter, ct);
        }

        public Task CreateAsync(LeadComment comment, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync($"{_settings.Host}/Api/Leads/Comments/Create", comment, ct);
        }
    }
}