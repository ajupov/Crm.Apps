using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Http;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.RequestParameters;
using Microsoft.Extensions.Options;
using UriBuilder = Ajupov.Utils.All.Http.UriBuilder;

namespace Crm.Apps.Clients.Products.Clients
{
    public class ProductChangesClient : IProductChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductChangesClient(IOptions<ClientsSettings> options, IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Products/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<ProductChange>> GetPagedListAsync(
            ProductChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ProductChange>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }
    }
}