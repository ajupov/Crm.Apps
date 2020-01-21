using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.v1.Models;
using Crm.Apps.Activities.v1.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityChangesService
    {
        Task<List<ActivityChange>> GetPagedListAsync(
            ActivityChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}