using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;

namespace Crm.Apps.Customers.Services
{
    public interface ICustomerChangesService
    {
        Task<CustomerChangeGetPagedListResponse> GetPagedListAsync(
            CustomerChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
