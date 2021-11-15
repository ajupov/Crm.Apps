using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Suppliers.Storages;
using Crm.Apps.Suppliers.V1.Requests;
using Crm.Apps.Suppliers.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Suppliers.Services
{
    public class SupplierChangesService : ISupplierChangesService
    {
        private readonly SuppliersStorage _storage;

        public SupplierChangesService(SuppliersStorage storage)
        {
            _storage = storage;
        }

        public async Task<SupplierChangeGetPagedListResponse> GetPagedListAsync(
            SupplierChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.SupplierChanges
                .AsNoTracking()
                .Where(x =>
                    (request.SupplierId.IsEmpty() || x.SupplierId == request.SupplierId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new SupplierChangeGetPagedListResponse
            {
                TotalCount = await changes
                    .CountAsync(ct),
                Changes = await changes
                    .SortBy(request.SortBy, request.OrderBy)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync(ct)
            };
        }
    }
}
