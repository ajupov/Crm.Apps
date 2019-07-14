using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;

namespace Crm.Apps.Accounts.Services
{
    public interface IAccountChangesService
    {
        Task<AccountChange[]> GetPagedListAsync(
            AccountChangeGetPagedListParameter parameter,
            CancellationToken ct);
    }
}