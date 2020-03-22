using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Deals.Storages;
using Crm.Apps.Deals.v1.Requests;
using Crm.Apps.Deals.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Deals.Services
{
    public class DealStatusChangesService : IDealStatusChangesService
    {
        private readonly DealsStorage _storage;

        public DealStatusChangesService(DealsStorage storage)
        {
            _storage = storage;
        }

        public async Task<DealStatusChangeGetPagedListResponse> GetPagedListAsync(
            DealStatusChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.DealStatusChanges
                .Where(x =>
                    (request.StatusId.IsEmpty() || x.StatusId == request.StatusId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new DealStatusChangeGetPagedListResponse
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