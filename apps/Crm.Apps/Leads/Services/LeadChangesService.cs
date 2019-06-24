using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Helpers;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;
using Crm.Apps.Leads.Storages;
using Crm.Utils.Guid;
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

        public Task<List<LeadChange>> GetPagedListAsync(LeadChangeGetPagedListParameter parameter, CancellationToken ct)
        {
            return _storage.LeadChanges.Where(x =>
                    (parameter.ChangerUserId.IsEmpty() || x.ChangerUserId == parameter.ChangerUserId) &&
                    (parameter.LeadId.IsEmpty() || x.LeadId == parameter.LeadId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}