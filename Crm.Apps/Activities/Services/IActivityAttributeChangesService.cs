using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Activities.Models;
using Crm.Apps.Activities.RequestParameters;

namespace Crm.Apps.Activities.Services
{
    public interface IActivityAttributeChangesService
    {
        Task<List<ActivityAttributeChange>> GetPagedListAsync(
            ActivityAttributeChangeGetPagedListRequestParameter request,
            CancellationToken ct);
    }
}