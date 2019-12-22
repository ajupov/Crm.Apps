using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.RequestParameters;

namespace Crm.Apps.Areas.Products.Services
{
    public interface IProductCategoryChangesService
    {
        Task<List<ProductCategoryChange>> GetPagedListAsync(
            ProductCategoryChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}