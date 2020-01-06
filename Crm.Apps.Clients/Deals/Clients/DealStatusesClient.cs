using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Deals.Models;
using Crm.Apps.Clients.Deals.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Deals.Clients
{
    public class DealStatusesClient : IDealStatusesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public DealStatusesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Deals/Statuses");
            _httpClientFactory = httpClientFactory;
        }

        public Task<DealStatus> GetAsync(Guid id, CancellationToken ct = default)
        {
            return _httpClientFactory.GetAsync<DealStatus>(UriBuilder.Combine(_url, "Get"), new {id}, ct);
        }

        public Task<List<DealStatus>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<DealStatus>>(UriBuilder.Combine(_url, "GetList"), ids, ct);
        }

        public Task<List<DealStatus>> GetPagedListAsync(
            DealStatusGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<DealStatus>>(UriBuilder.Combine(_url, "GetPagedList"), request,
                ct);
        }

        public Task<Guid> CreateAsync(DealStatus status, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<Guid>(UriBuilder.Combine(_url, "Create"), status, ct);
        }

        public Task UpdateAsync(DealStatus status, CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync(UriBuilder.Combine(_url, "Update"), status, ct);
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