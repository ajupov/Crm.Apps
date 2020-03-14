using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Ajupov.Utils.All.Sorting;
using Crm.Apps.Leads.Storages;
using Crm.Apps.Leads.v1.Requests;
using Crm.Apps.Leads.v1.Responses;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Leads.Services
{
    public class LeadChangesService : ILeadChangesService
    {
        private readonly LeadsStorage _storage;

        public LeadChangesService(LeadsStorage storage)
        {
            _storage = storage;
        }

        public async Task<LeadChangeGetPagedListResponse> GetPagedListAsync(
            LeadChangeGetPagedListRequest request,
            CancellationToken ct)
        {
            var changes = _storage.LeadChanges
                .Where(x =>
                    (request.LeadId.IsEmpty() || x.LeadId == request.LeadId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate));

            return new LeadChangeGetPagedListResponse
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