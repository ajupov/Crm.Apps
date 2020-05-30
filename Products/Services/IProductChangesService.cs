using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Products.V1.Requests;
using Crm.Apps.Products.V1.Responses;

namespace Crm.Apps.Products.Services
{
    public interface IProductChangesService
    {
        Task<ProductChangeGetPagedListResponse> GetPagedListAsync(
            ProductChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
