using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;

namespace Crm.Apps.Customers.Services
{
    public interface ICustomerSourceChangesService
    {
        Task<CustomerSourceChangeGetPagedListResponse> GetPagedListAsync(
            CustomerSourceChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
