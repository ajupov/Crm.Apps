using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;

namespace Crm.Apps.Customers.Services
{
    public interface ICustomerCommentsService
    {
        Task<CustomerCommentGetPagedListResponse> GetPagedListAsync(
            CustomerCommentGetPagedListRequest request,
            CancellationToken ct);

        Task CreateAsync(Guid userId, CustomerComment comment, CancellationToken ct);
    }
}
