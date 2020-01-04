using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.RequestParameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadAttributeChangesService
    {
        Task<List<LeadAttributeChange>> GetPagedListAsync(
            LeadAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}