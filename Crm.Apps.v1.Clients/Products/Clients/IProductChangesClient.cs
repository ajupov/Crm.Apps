using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.v1.Clients.Products.Models;
using Crm.Apps.v1.Clients.Products.RequestParameters;

namespace Crm.Apps.v1.Clients.Products.Clients
{
    public interface IProductChangesClient
    {
        Task<List<ProductChange>> GetPagedListAsync(
            string accessToken,
            ProductChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}