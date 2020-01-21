using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.v1.Models;
using Crm.Apps.Leads.v1.RequestParameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadSourceChangesService
    {
        Task<List<LeadSourceChange>> GetPagedListAsync(
            LeadSourceChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}