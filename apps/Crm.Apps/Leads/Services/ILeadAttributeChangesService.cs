using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.Models;
using Crm.Apps.Leads.Parameters;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadAttributeChangesService
    {
        Task<List<LeadAttributeChange>> GetPagedListAsync(LeadAttributeChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}