using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadChangesService
    {
        Task<List<LeadChange>> GetPagedListAsync(LeadChangeGetPagedListRequestParameter request, CancellationToken ct);
    }
}