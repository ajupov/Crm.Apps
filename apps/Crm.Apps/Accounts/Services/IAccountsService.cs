using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Apps.Accounts.Models;
using Crm.Apps.Accounts.Parameters;

namespace Crm.Apps.Accounts.Services
{
    public interface IAccountsService
    {
        Task<Account> GetAsync(Guid id, CancellationToken ct);

        Task<List<Account>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Account>> GetPagedListAsync(AccountGetPagedListParameter parameter, CancellationToken ct);

        Task<Guid> CreateAsync(Guid userId, Account account, CancellationToken ct);

        Task UpdateAsync(Guid userId, Account oldAccount, Account newAccount, CancellationToken ct);

        Task LockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(Guid userId, IEnumerable<Guid> ids, CancellationToken ct);
    }
}