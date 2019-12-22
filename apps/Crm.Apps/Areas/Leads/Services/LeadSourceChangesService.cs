using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Guid;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.RequestParameters;
using Crm.Apps.Areas.Leads.Storages;
using Crm.Apps.Utils;
using Microsoft.EntityFrameworkCore;

namespace Crm.Apps.Areas.Leads.Services
{
    public class LeadSourceChangesService : ILeadSourceChangesService
    {
        private readonly LeadsStorage _storage;

        public LeadSourceChangesService(LeadsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<LeadSourceChange>> GetPagedListAsync(
            LeadSourceChangeGetPagedListRequestParameter request,
            CancellationToken ct)
        {
            return _storage.LeadSourceChanges
                .AsNoTracking()
                .Where(x =>
                    (request.ChangerUserId.IsEmpty() || x.ChangerUserId == request.ChangerUserId) &&
                    (request.SourceId.IsEmpty() || x.SourceId == request.SourceId) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate))
                .SortBy(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }
    }
}