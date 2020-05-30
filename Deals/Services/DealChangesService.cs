using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Deals.V1.Requests;
using Crm.Apps.Deals.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Deals.Services
{
    public class DealChangesService : IDealChangesService
    {
        private readonly DealsStorage _storage;

        public DealChangesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public async Task<DealChangeGetPagedListResponse> GetPagedListAsync(
            DealChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.DealChanges
                .Where(x =>
                    (request.DealId.IsEmpty() || x.DealId == request.DealId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new DealChangeGetPagedListResponse
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
