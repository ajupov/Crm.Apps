using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Suppliers.Models;
using Crm.Apps.Suppliers.V1.Requests;
using Crm.Apps.Suppliers.V1.Responses;

namespace Crm.Apps.Suppliers.Services
{
    public interface ISuppliersService
    {
        Task<Supplier> GetAsync(Guid id, bool isTrackChanges, CancellationToken ct);

        Task<List<Supplier>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<SupplierGetPagedListResponse> GetPagedListAsync(
            Guid accountId,
            SupplierGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Supplier supplier, CancellationToken ct);

        Task UpdateAsync(Guid userId, Supplier oldSupplier, Supplier newSupplier, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}
