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
    public class SupplierAttributeChangesService : ISupplierAttributeChangesService
    {
        private readonly SuppliersStorage _storage;

        public SupplierAttributeChangesService(SuppliersStorage storage)
        {
            _storage = storage;
        }

        public async Task<SupplierAttributeChangeGetPagedListResponse> GetPagedListAsync(
            SupplierAttributeChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.SupplierAttributeChanges
                .AsNoTracking()
                .Where(x =>
                    (request.AttributeId.IsEmpty() || x.AttributeId == request.AttributeId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new SupplierAttributeChangeGetPagedListResponse
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
