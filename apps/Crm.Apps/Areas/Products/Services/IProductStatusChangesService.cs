using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Products.Models;
using Crm.Apps.Areas.Products.Parameters;

namespace Crm.Apps.Areas.Products.Services
{
    public interface IProductStatusChangesService
    {
        Task<List<ProductStatusChange>> GetPagedListAsync(
            ProductStatusChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}