using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.Parameters;

namespace Crm.Apps.Areas.Leads.Services
{
    public interface ILeadChangesService
    {
        Task<List<LeadChange>> GetPagedListAsync(LeadChangeGetPagedListParameter parameter, CancellationToken ct);
    }
}