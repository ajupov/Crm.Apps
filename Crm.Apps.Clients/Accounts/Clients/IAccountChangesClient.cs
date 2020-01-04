using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Clients.Accounts.Models;
using Crm.Apps.Clients.Accounts.RequestParameters;

namespace Crm.Apps.Clients.Accounts.Clients
{
    public interface IAccountChangesClient
    {
        Task<List<AccountChange>> GetPagedListAsync(
            AccountChangeGetPagedListRequestParameter request,
            CancellationToken ct = default);
    }
}