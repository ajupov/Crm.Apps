using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;

namespace Crm.Apps.Customers.Services
{
    public interface ICustomersService
    {
        Task<Customer> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<Customer>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<CustomerGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            CustomerGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Customer customer, CancellationToken ct);

        Task UpdateAsync(Guid userId, Customer oldCustomer, Customer newCustomer, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
