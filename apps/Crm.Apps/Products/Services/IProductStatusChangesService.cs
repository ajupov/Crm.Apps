using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.Models;
using Crm.Apps.Products.Parameters;

namespace Crm.Apps.Products.Services
{
    public interface IProductStatusChangesService
    {
        Task<List<ProductStatusChange>> GetPagedListAsync(ProductStatusChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}