using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;

namespace Crm.Apps.Customers.Services
{
    public interface ICustomerSourcesService
    {
        Task<CustomerSource> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<CustomerSource>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<CustomerSourceGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            CustomerSourceGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, CustomerSource source, CancellationToken ct);

        Task UpdateAsync(Guid userId, CustomerSource oldSource, CustomerSource newSource, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
