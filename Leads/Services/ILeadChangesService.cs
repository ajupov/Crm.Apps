using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Leads.v1.Requests;
using Crm.Apps.Leads.v1.Responses;

namespace Crm.Apps.Leads.Services
{
    public interface ILeadChangesService
    {
        Task<LeadChangeGetPagedListResponse> GetPagedListAsync(
            LeadChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}