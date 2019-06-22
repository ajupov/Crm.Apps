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
    public class LeadAttributeChangesService : ILeadAttributeChangesService
    {
        private readonly LeadsStorage _storage;

        public LeadAttributeChangesService(LeadsStorage storage)
        {
            _storage = storage;
        }

        public Task<List<LeadAttributeChange>> GetPagedListAsync(LeadAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct)
        {
            return _storage.LeadAttributeChanges.Where(x =>
                    (!parameter.ChangerUserId.HasValue || x.ChangerUserId == parameter.ChangerUserId) &&
                    (!parameter.AttributeId.HasValue || x.AttributeId == parameter.AttributeId) &&
                    (!parameter.MinCreateDate.HasValue || x.CreateDateTime >= parameter.MinCreateDate) &&
                    (!parameter.MaxCreateDate.HasValue || x.CreateDateTime <= parameter.MaxCreateDate))
                .Sort(parameter.SortBy, parameter.OrderBy)
                .Skip(parameter.Offset)
                .Take(parameter.Limit)
                .ToListAsync(ct);
        }
    }
}