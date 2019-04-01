using System.Threading;
using System.Threading.Tasks;

namespace Crm.Clients.Accounts.Clients.AccountsDefault
{
    public interface IAccountsDefaultClient
    {
        Task StatusAsync(CancellationToken ct = default);
    }
}