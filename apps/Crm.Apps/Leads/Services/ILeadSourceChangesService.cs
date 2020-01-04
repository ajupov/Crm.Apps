using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadSourceChangesService
    {
        Task<List<LeadSourceChange>> GetPagedListAsync(
            LeadSourceChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}