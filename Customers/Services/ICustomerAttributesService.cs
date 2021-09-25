using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Customers.Models;
using Crm.Apps.Customers.V1.Requests;
using Crm.Apps.Customers.V1.Responses;

namespace Crm.Apps.Customers.Services
{
    public interface ICustomerAttributesService
    {
        Task<CustomerAttribute> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<CustomerAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<CustomerAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            CustomerAttributeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, CustomerAttribute attribute, CancellationToken ct);

        Task UpdateAsync(Guid userId, CustomerAttribute oldAttribute, CustomerAttribute newAttribute, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
