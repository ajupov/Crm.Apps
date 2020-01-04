using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;
using Crm.Clients.Accounts.RequestParameters;

namespace Crm.Clients.Accounts.Clients
{
    public interface IAccountChangesClient
    {
        Task<AccountChange[]> GetPagedListAsync(
            AccountChangeGetPagedListRequest request,
            CancellationToken ct = default);
    }
}