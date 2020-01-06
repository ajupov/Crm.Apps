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
    public class ProductAttributeChangesClient : IProductAttributeChangesClient
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductAttributeChangesClient(IOptions<ClientsSettings> options,
            IHttpClientFactory httpClientFactory)
        {
            _url = UriBuilder.Combine(options.Value.Host, "Products/Attributes/Changes");
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<ProductAttributeChange>> GetPagedListAsync(
            ProductAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default)
        {
            return _httpClientFactory.PostAsync<List<ProductAttributeChange>>(
                UriBuilder.Combine(_url, "GetPagedList"), request, ct);
        }
    }
}