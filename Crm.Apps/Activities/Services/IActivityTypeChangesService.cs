using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityTypeChangesService
    {
        Task<List<ActivityTypeChange>> GetPagedListAsync(
            ActivityTypeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}