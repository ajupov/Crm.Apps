using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Areas.Leads.Models;
using Crm.Apps.Areas.Leads.RequestParameters;

namespace Crm.Apps.Areas.Leads.Services
{
    public interface ILeadAttributeChangesService
    {
        Task<List<LeadAttributeChange>> GetPagedListAsync(
            LeadAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}