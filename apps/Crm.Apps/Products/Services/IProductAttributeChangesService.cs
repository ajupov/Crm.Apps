using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.RequestParameters;

namespace Crm.Apps.Products.Services
{
    public interface IProductAttributeChangesService
    {
        Task<List<ProductAttributeChange>> GetPagedListAsync(
            ProductAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}