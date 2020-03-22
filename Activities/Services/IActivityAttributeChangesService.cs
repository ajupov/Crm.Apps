using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.v1.Requests;
using Crm.Apps.Activities.v1.Responses;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityAttributeChangesService
    {
        Task<ActivityAttributeChangeGetPagedListResponse> GetPagedListAsync(
            ActivityAttributeChangeGetPagedListRequest request,
            CancellationToken ct);
    }
}