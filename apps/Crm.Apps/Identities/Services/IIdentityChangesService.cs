using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Identities.Models;
using Crm.Apps.Identities.Parameters;

namespace Crm.Apps.Identities.Services
{
    public interface IIdentityChangesService
    {
        Task<List<IdentityChange>> GetPagedListAsync(IdentityChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}