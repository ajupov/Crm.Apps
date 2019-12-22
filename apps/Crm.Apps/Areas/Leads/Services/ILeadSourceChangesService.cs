using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;

namespace Crm.Apps.Areas.Leads.Services
{
    public interface ILeadSourceChangesService
    {
        Task<List<LeadSourceChange>> GetPagedListAsync(LeadSourceChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}