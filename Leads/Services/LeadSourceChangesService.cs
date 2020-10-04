using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Leads.V1.Requests;
using Crm.Apps.Leads.V1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Leads.Services
{
    public class LeadSourceChangesService : ILeadSourceChangesService
    {
        private readonly LeadsStorage _storage;

        public LeadSourceChangesService(LeadsStorage storage)
        {
            _storage = storage;
        }

        public async Task<LeadSourceChangeGetPagedListResponse> GetPagedListAsync(
            LeadSourceChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.LeadSourceChanges
                .AsNoTracking()
                .Where(x =>
                    (request.SourceId.IsEmpty() || x.SourceId == request.SourceId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new LeadSourceChangeGetPagedListResponse
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
