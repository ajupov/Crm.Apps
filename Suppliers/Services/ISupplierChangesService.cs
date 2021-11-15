using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Suppliers.V1.Requests;
using Crm.Apps.Suppliers.V1.Responses;

namespace Crm.Apps.Suppliers.Services
{
    public interface ISupplierChangesService
    {
        Task<SupplierChangeGetPagedListResponse> GetPagedListAsync(
            SupplierChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
