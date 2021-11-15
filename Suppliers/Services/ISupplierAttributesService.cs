using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Suppliers.Models;
using Crm.Apps.Suppliers.V1.Requests;
using Crm.Apps.Suppliers.V1.Responses;

namespace Crm.Apps.Suppliers.Services
{
    public interface ISupplierAttributesService
    {
        Task<SupplierAttribute> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<SupplierAttribute>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<SupplierAttributeGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            SupplierAttributeGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, SupplierAttribute attribute, CancellationToken ct);

        Task UpdateAsync(
            Guid userId,
            SupplierAttribute oldAttribute,
            SupplierAttribute newAttribute,
            CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
