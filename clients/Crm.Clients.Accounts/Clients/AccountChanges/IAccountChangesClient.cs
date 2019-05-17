using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.Clients.AccountChanges
{
    public interface IAccountChangesClient
    {
        Task<List<AccountChange>> GetPagedListAsync(Guid? changerUserId = default, Guid? accountId = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);
    }
}