using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.v1.Requests;
using Crm.Apps.Leads.v1.Responses;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadSourceChangesService
    {
        Task<LeadSourceChangeGetPagedListResponse> GetPagedListAsync(
            LeadSourceChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}