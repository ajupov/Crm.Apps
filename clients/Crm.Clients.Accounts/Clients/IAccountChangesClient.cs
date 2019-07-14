using System;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.Clients
{
    public interface IAccountChangesClient
    {
        Task<AccountChange[]> GetPagedListAsync(
            Guid accountId,
            Guid? changerUserId = default,
            DateTime? minCreateDate = default,
            DateTime? maxCreateDate = default,
            int offset = default,
            int limit = 10,
            string sortBy = "CreateDateTime",
            string orderBy = "desc",
            CancellationToken ct = default);
    }
}