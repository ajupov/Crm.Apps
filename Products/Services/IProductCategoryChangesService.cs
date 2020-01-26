using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.v1.Models;
using Crm.Apps.Products.v1.RequestParameters;

namespace Crm.Apps.Products.Services
{
    public interface IProductCategoryChangesService
    {
        Task<List<ProductCategoryChange>> GetPagedListAsync(
            ProductCategoryChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}