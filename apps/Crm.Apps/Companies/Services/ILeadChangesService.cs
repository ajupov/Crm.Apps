using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadChangesService
    {
        Task<List<LeadChange>> GetPagedListAsync(LeadChangeGetPagedListParameter parameter, CancellationToken ct);
    }
}