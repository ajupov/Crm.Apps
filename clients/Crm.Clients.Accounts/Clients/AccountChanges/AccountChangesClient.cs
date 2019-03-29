using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Clients.Accounts.Clients.AccountChanges
{
    public class AccountChangesClient : HttpClient, IAccountChangesClient
    {
        public AccountChangesClient(string host) : base($"{host}/api/v1/AccountChanges")
        {
        }

        public Task<List<AccountChange>> GetListAsync(int accountId, int offset = 0, int limit = 10)
        {
            return GetAsync<List<AccountChange>>("GetList", new {accountId, offset, limit});
        }
    }
}