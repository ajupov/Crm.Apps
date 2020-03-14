using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.v1.Requests;
using Crm.Apps.Leads.v1.Responses;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadAttributeChangesService
    {
        Task<LeadAttributeChangeGetPagedListResponse> GetPagedListAsync(
            LeadAttributeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}