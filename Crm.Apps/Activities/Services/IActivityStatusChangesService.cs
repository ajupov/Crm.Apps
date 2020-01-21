using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityStatusChangesService
    {
        Task<List<ActivityStatusChange>> GetPagedListAsync(
            ActivityStatusChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}