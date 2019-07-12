using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.Parameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityStatusChangesService
    {
        Task<List<ActivityStatusChange>> GetPagedListAsync(ActivityStatusChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}