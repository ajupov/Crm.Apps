using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Helpers;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;
using Crm.Apps.Leads.Storages;
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

        public Task<List<LeadSourceChange>> GetPagedListAsync(LeadSourceChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.LeadSourceChanges.Where(x =>
                    (!parameter.ChangerUserId.HasValue || x.ChangerUserId == parameter.ChangerUserId) &&
                    (!parameter.SourceId.HasValue || x.SourceId == parameter.SourceId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}