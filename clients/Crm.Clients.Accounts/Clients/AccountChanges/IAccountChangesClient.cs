using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Clients.Accounts.Clients.AccountChanges
{
    public interface IAccountChangesClient
    {
        Task<List<AccountChange>> GetListAsync(int accountId, int offset = 0, int limit = 10);
    }
}