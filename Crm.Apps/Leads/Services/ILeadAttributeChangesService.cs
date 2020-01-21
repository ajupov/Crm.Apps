using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.v1.Models;
using Crm.Apps.Leads.v1.RequestParameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadAttributeChangesService
    {
        Task<List<LeadAttributeChange>> GetPagedListAsync(
            LeadAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}