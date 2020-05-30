using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.V1.Requests;
using Crm.Apps.Leads.V1.Responses;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadChangesService
    {
        Task<LeadChangeGetPagedListResponse> GetPagedListAsync(
            LeadChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}
