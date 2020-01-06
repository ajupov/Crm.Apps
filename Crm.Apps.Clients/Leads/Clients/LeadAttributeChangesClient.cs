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
    public class LeadAttributeChangesClient : ILeadAttributeChangesClient
    {
        private readonly LeadsClientSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public LeadAttributeChangesClient(IOptions<LeadsClientSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, );
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<LeadAttributeChange>> GetPagedListAsync(Guid? changerUserId = default,
            Guid? attributeId = default, DateTime? minCreateDate = default, DateTime? maxCreateDate = default,
            int offset = default, int limit = 10, string sortBy = default, string orderBy = default,
            CancellationToken ct = default)
        {
            var parameter = new
            {
                ChangerUserId = changerUserId,
                AttributeId = attributeId,
                MinCreateDate = minCreateDate,
                MaxCreateDate = maxCreateDate,
                Offset = offset,
                Limit = limit,
                SortBy = sortBy,
                OrderBy = orderBy
            };

            return _httpClientFactory.PostAsync<List<LeadAttributeChange>>(
                $"{_settings.Host}/Api/Leads/Attributes/Changes/GetPagedList", parameter, ct);
        }
    }
}