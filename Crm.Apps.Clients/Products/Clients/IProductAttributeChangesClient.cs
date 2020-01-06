using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Products.Models;
using Crm.Apps.Clients.Products.RequestParameters;

namespace Crm.Apps.Clients.Products.Clients
{
    public interface IProductAttributeChangesClient
    {
        Task<List<ProductAttributeChange>> GetPagedListAsync(
            ProductAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}