using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Clients.Accounts.Models;

namespace Crm.Clients.Accounts.Clients.Accounts
{
    public interface IAccountsClient
    {
        Task<Account> GetAsync(Guid id, CancellationToken ct = default);

        Task<List<Account>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<List<Account>> GetPagedListAsync(bool? isLocked = default, bool? isDeleted = default,
            DateTime? minCreateDate = default, DateTime? maxCreateDate = default, int offset = default,
            int limit = 10, string sortBy = default, string orderBy = default, CancellationToken ct = default);

        Task<Guid> CreateAsync(Account account, CancellationToken ct = default);

        Task UpdateAsync(Account account, CancellationToken ct = default);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    }
}